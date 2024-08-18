using Core.Domain.Entities;

namespace Core.Domain.RepositoryContracts
{
    public interface IApiKeysRepository
    {
        string CreateKey(APIKey apiKey);
    }
}
