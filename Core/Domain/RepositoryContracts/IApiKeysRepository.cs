using Core.Domain.Entities;

namespace Core.Domain.RepositoryContracts
{
    public interface IApiKeysRepository
    {
        Task<string> CreateKey(APIKey apiKey);
        Task<bool> DeactivateApiKey(APIKey apiKey);
        Task<bool> ActivateApiKey(APIKey apiKey);
        Task<bool> DeleteApiKey(APIKey apiKey);
        Task<APIKey?> GetApiKeyByKey(string key);
        Task<APIKey?> GetApiKeyByClientName(string clientName);
    }
}
