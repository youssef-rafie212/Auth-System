using Core.Domain.Entities;

namespace Core.Domain.RepositoryContracts
{
    public interface IApiKeysRepository
    {
        Task<string> CreateKey(APIKey apiKey);

        Task<APIKey?> GetKey(string key);
    }
}
