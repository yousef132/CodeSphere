using CodeSphere.Domain.Responses.ElasticSearchResponses;

namespace CodeSphere.Domain.Abstractions.Repositories
{
    public interface IElasticSearchRepository
    {
        Task InitializeIndexes();
        Task<bool> IndexDocumentAsync<T>(T document, string indexName) where T : class;

        Task<bool> BulkIndexDocumentsAsync<T>(IEnumerable<T> documents, string indexName) where T : class;

        Task<IEnumerable<ProblemDocument>> SearchProblemsAsync(string? searchText, List<int>? topics, int? difficulty);
        Task<IEnumerable<ProblemDocument>> SearchProblemsAsync(string searchText);


        Task<IEnumerable<BlogDocument>> SearchBlogsAsync<T>(string searchText, List<string> tags, string indexName);

        Task<bool> UpdateDocumentAsync<T>(T document, string documentId, string indexName) where T : class;

        Task<bool> DeleteDocumentAsync(string documentId, string indexName);

        Task<T?> GetDocumentByIdAsync<T>(string documentId, string indexName) where T : class;

        Task<bool> IndexExistsAsync(string indexName);

        Task<bool> CreateIndexIfNotExistsAsync(string indexName);

        Task<bool> DeleteIndexAsync(string indexName);
    }

}
