using CodeSphere.Application.Helpers;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
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
        private readonly HttpClient httpClient;

        public ElasticSearchRepository(IOptions<ElasticSetting> options, HttpClient httpClient)
        {
            this.elasticSetting = options.Value;

            var settings = new ConnectionSettings(new Uri(elasticSetting.Url))
                .DefaultIndex(elasticSetting.DefaultIndex);


            this._elasticClient = new ElasticClient(settings);
            this.httpClient = httpClient;
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

        public async Task<(IEnumerable<ProblemDocument>, int)> SearchProblemsAsync(
            string? searchText,
            List<int>? topics,
            Difficulty? difficulty,
            SortBy sortBy,
            Order order,
            int pageNumber,
            int pageSize)
        {
            #region nest


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
                                 .Sort(srt =>
                                        order == Order.Ascending
                                            ? srt.Ascending(ElasticHelper.GetSortField(sortBy))
                                            : srt.Descending(ElasticHelper.GetSortField(sortBy))
                                 )
                                 .From((pageNumber - 1) * pageSize)
                                 .Size(pageSize)
                             );

            int TotalNumberOfPages = (int)Math.Ceiling((double)fuzzySearchResponse.Total / pageSize);
            return (fuzzySearchResponse.Hits.Select(hit => hit.Source), TotalNumberOfPages);
            #endregion

            #region api call

            //var queryJson = $@"
            //              {{
            //                  ""size"": 1000,
            //                  ""query"": {{
            //                      ""bool"": {{
            //                          ""must"": [
            //                              {(string.IsNullOrEmpty(searchText) ? "" : $@"
            //                              {{
            //                                  ""match"": {{
            //                                      ""Name"": {{
            //                                          ""query"": ""{searchText}"",
            //                                          ""fuzziness"": 2
            //                                      }}
            //                                  }}
            //                              }}")}
            //                          ],
            //                          ""filter"": [
            //                              {(topics != null && topics.Any() ? $@"
            //                              {{
            //                                  ""terms"": {{
            //                                      ""Topics"": [{string.Join(",", topics)}]
            //                                  }}
            //                              }}" : "")}
            //                              {(difficulty != null ? $@"
            //                              {{
            //                                  ""term"": {{
            //                                      ""Difficulty"": ""{difficulty}""
            //                                  }}
            //                              }}" : "")}
            //                          ]
            //                      }}
            //                  }}
            //              }}";

            //var content = new StringContent(queryJson, Encoding.UTF8, "application/json");
            //var response = await httpClient.PostAsync(elasticSetting.Url + "/problems/_search", content);
            //response.EnsureSuccessStatusCode();

            //// Parse the response using the updated class structure
            //var responseString = await response.Content.ReadAsStringAsync();
            //var searchResponse = JsonConvert.DeserializeObject<ElasticSearchResponse<ProblemDocument>>(responseString);

            //// Return the documents (ProblemDocument) from the hits
            //return searchResponse?.Hits?.Hits?.Select(h => h._source) ?? Enumerable.Empty<ProblemDocument>();
            #endregion


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
            var defaultIndexExists = await _elasticClient.Indices.ExistsAsync(elasticSetting.DefaultIndex);
            var secondaryIndexExists = await _elasticClient.Indices.ExistsAsync(elasticSetting.SecondryIndex);

            if (!defaultIndexExists.Exists)
            {
                var createProblemsIndexResponse = await _elasticClient.Indices.CreateAsync(elasticSetting.DefaultIndex, c => c
                    .Map<ProblemDocument>(m => m.AutoMap())
                );

                if (!createProblemsIndexResponse.IsValid)
                {
                    throw new Exception($"Failed to create index {elasticSetting.DefaultIndex}: {createProblemsIndexResponse.DebugInformation}");
                }
            }

            if (!secondaryIndexExists.Exists)
            {
                var createBlogIndexResponse = await _elasticClient.Indices.CreateAsync(elasticSetting.SecondryIndex, c => c
                    .Map<BlogDocument>(m => m.AutoMap())
                );

                if (!createBlogIndexResponse.IsValid)
                {
                    throw new Exception($"Failed to create index {elasticSetting.SecondryIndex}: {createBlogIndexResponse.DebugInformation}");
                }
            }
        }


    }

}
