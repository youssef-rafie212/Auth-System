using Core.Domain.Entities;
using Core.Domain.RepositoryContracts;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ApiKeysRepository : IApiKeysRepository
    {
        private readonly AppDBContext _db;

        public ApiKeysRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<bool> ActivateApiKey(APIKey apiKey)
        {
            if (apiKey.IsActive == true) return false;

            apiKey.IsActive = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<string> CreateKey(APIKey apiKey)
        {
            _db.APIKeys.Add(apiKey);
            await _db.SaveChangesAsync();

            return apiKey.Key!;
        }

        public async Task<bool> DeactivateApiKey(APIKey apiKey)
        {
            if (apiKey.IsActive == false) return false;

            apiKey.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteApiKey(APIKey apiKey)
        {
            _db.APIKeys.Remove(apiKey);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<APIKey?> GetApiKeyByClientName(string clientName)
        {
            return await _db.APIKeys.FirstOrDefaultAsync(k => k.ClientName == clientName);
        }

        public async Task<APIKey?> GetApiKeyByKey(string key)
        {
            return await _db.APIKeys.FirstOrDefaultAsync(k => k.Key == key);
        }

    }
}
