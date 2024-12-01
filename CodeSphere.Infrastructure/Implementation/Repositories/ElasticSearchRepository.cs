using CodeSphere.Application.Helpers;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.ElasticSearchResponses;
using Microsoft.Extensions.Options;
using Nest;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        private readonly ElasticClient _elasticClient;
        private readonly ElasticSetting elasticSetting;
        public ElasticSearchRepository(IOptions<ElasticSetting> options)
        {
            this.elasticSetting = options.Value;

            var settings = new ConnectionSettings(new Uri(elasticSetting.Url))
                .DefaultIndex(elasticSetting.DefaultIndex);

            this._elasticClient = new ElasticClient(settings);
        }

        public async Task<bool> IndexDocumentAsync<T>(T document, string indexName) where T : class
        {
            var response = await _elasticClient.IndexAsync(document, i => i.Index(indexName));
            return response.IsValid;
        }

        public async Task<bool> BulkIndexDocumentsAsync<T>(IEnumerable<T> documents, string indexName) where T : class
        {
            var response = await _elasticClient.BulkAsync(b => b
                .Index(indexName)
                .IndexMany(documents));
            return response.IsValid;
        }

        public async Task<IEnumerable<ProblemDocument>> SearchProblemsAsync(string searchText, List<string> topics, string difficulty)
        {

            var fuzzySearchResponse = _elasticClient.Search<ProblemDocument>(s => s
                                 .Index(ElasticSearchIndexes.Problems)
                                 .Query(q => q
                                     .Bool(b => b
                                         .Must(
                                             m => m.Match(mq => mq
                                                 .Field(f => f.Name) // Fuzzy search on Name
                                                 .Query(searchText)
                                                 .Fuzziness(Nest.Fuzziness.EditDistance(2))
                                             )
                                         )
                                         .Filter(
                                             f => f.Terms(t => t
                                                 .Field(ff => ff.Topics) // Exact match on topics
                                                 .Terms(topics)
                                             ),
                                             f => f.Term(t => t
                                                 .Field(ff => ff.Difficulty) // Exact match on difficulty
                                                 .Value(difficulty)
                                             )
                                         )
                                     )
                                 )
                             );

            return fuzzySearchResponse.Hits.Select(hit => hit.Source);

        }

        public async Task<IEnumerable<ProblemDocument>> SearchProblemsAsync(string searchText)
        {

            var fuzzySearchResponse = _elasticClient.Search<ProblemDocument>(s => s
                                 .Index(ElasticSearchIndexes.Problems)
                                 .Query(q => q
                                     .Bool(b => b
                                         .Must(
                                             m => m.Match(mq => mq
                                                 .Field(f => f.Name) // Fuzzy search on Name
                                                 .Query(searchText)
                                                 .Fuzziness(Nest.Fuzziness.EditDistance(2))
                                             )
                                         )
                                     )
                                 )
                             );

            return fuzzySearchResponse.Hits.Select(hit => hit.Source);

        }

        public async Task<IEnumerable<BlogDocument>> SearchBlogsAsync<T>(string searchText, List<string> tags, string indexName)
        {
            var response = await _elasticClient.SearchAsync<BlogDocument>(s => s
                .Index(indexName)
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            m => m.Match(mq => mq.Field("content").Query(searchText)),
                            m => m.Terms(tq => tq.Field("tags").Terms(tags))
                        )
                    )
                )
            );

            return response.Documents;
        }

        public async Task<bool> UpdateDocumentAsync<T>(T document, string documentId, string indexName) where T : class
        {
            var response = await _elasticClient.UpdateAsync<T>(documentId, u => u
                .Index(indexName)
                .Doc(document));
            return response.IsValid;
        }
        public async Task<T?> GetDocumentByIdAsync<T>(string documentId, string indexName) where T : class
        {
            var response = await _elasticClient.GetAsync<T>(documentId, g => g.Index(indexName));
            return response.Found ? response.Source : null;
        }

        public async Task<bool> IndexExistsAsync(string indexName)
        {
            var response = await _elasticClient.Indices.ExistsAsync(indexName);
            return response.Exists;
        }

        public async Task<bool> CreateIndexIfNotExistsAsync(string indexName)
        {
            var exists = await IndexExistsAsync(indexName);
            if (!exists)
            {
                var response = await _elasticClient.Indices.CreateAsync(indexName, c => c
                    .Map(m => m.AutoMap()));
                return response.IsValid;
            }
            return true;
        }

        public async Task<bool> DeleteIndexAsync(string indexName)
        {
            var response = await _elasticClient.Indices.DeleteAsync(indexName);
            return response.IsValid;
        }

        public Task<bool> DeleteDocumentAsync(string documentId, string indexName)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeIndexes()
        {
            if (!_elasticClient.Indices.Exists(elasticSetting.DefaultIndex).Exists)
            {

                var createProblemsIndexResponse = await _elasticClient.Indices.CreateAsync(elasticSetting.DefaultIndex, c => c
                                    .Map<ProblemDocument>(m => m
                                        .AutoMap()
                                    )
                                );

            }
            if (!_elasticClient.Indices.Exists(elasticSetting.SecondryIndex).Exists)
            {

                var createBlogIndexResponse = await _elasticClient.Indices.CreateAsync(elasticSetting.SecondryIndex, c => c
                                    .Map<BlogDocument>(m => m
                                        .AutoMap()
                                    )
                                );

            }
        }
    }

}
