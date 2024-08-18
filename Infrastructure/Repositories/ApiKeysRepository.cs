using Core.Domain.Entities;
using Core.Domain.RepositoryContracts;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    public class ApiKeysRepository : IApiKeysRepository
    {
        private readonly AppDBContext _db;

        public ApiKeysRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<string> CreateKey(APIKey apiKey)
        {
            _db.APIKeys.Add(apiKey);
            await _db.SaveChangesAsync();

            return apiKey.Key!;
        }
    }
}
