using Core.Domain.Entities;

namespace Core.Domain.RepositoryContracts
{
    public interface IApiKeysRepository
    {
        Task<string> CreateKey(APIKey apiKey);

        Task<APIKey?> GetApiKeyByKey(string key);
        Task<APIKey?> GetApiKeyByClientName(string clientName);
    }
}
