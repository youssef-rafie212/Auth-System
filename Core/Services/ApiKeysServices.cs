using Core.Domain.Entities;
using Core.Domain.RepositoryContracts;
using Core.DTO.ApiKeyDtos;
using Core.Helpers;
using Core.ServiceContracts;

namespace Core.Services
{
    public class ApiKeysServices : IApiKeysServices
    {
        private readonly IApiKeysRepository _repo;
        private readonly ServicesHelpers _servicesHelpers;

        public ApiKeysServices(IApiKeysRepository repo, ServicesHelpers servicesHelpers)
        {
            _repo = repo;
            _servicesHelpers = servicesHelpers;
        }

        public async Task<bool> ActivateApiKey(ActivateApiKeyDto activateApiKeyDto)
        {
            APIKey? apiKey = await _repo.GetApiKeyByKey(activateApiKeyDto.Key!);
            if (apiKey == null) return false;

            return await _repo.ActivateApiKey(apiKey);
        }

        public async Task<bool> DeactivateApiKey(DeactivateApiKeyDto deactivateApiKeyDto)
        {
            APIKey? apiKey = await _repo.GetApiKeyByKey(deactivateApiKeyDto.Key!);
            if (apiKey == null) return false;

            return await _repo.DeactivateApiKey(apiKey);
        }

        public async Task<bool> DeleteApiKey(DeleteApiKeyDto deleteApiKeyDto)
        {
            APIKey? apiKey = await _repo.GetApiKeyByKey(deleteApiKeyDto.Key!);
            if (apiKey == null) return false;

            return await _repo.DeleteApiKey(apiKey);
        }

        public async Task<APIKey> GetAPIKey(string key)
        {
            APIKey? apiKey = await _repo.GetApiKeyByKey(key);

            if (apiKey == null) throw new ArgumentException("Key doesn't exist");

            if (!apiKey.IsActive) throw new ArgumentException("Key is no longer active");

            return apiKey;
        }

        public async Task<GetApiKeyResponseDto> GetNewApiKey(GetApiKeyRequestDto getApiKeyDto)
        {
            APIKey? clientNameExists = await _repo.GetApiKeyByClientName(getApiKeyDto.ClientName!);

            if (clientNameExists != null) throw new ArgumentException("Client name already exists.");

            string key = await _repo.CreateKey(new APIKey
            {
                Id = Guid.NewGuid(),
                ClientName = getApiKeyDto.ClientName!,
                Key = _servicesHelpers.GenerateUniqueString(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                TenantId = Guid.NewGuid().ToString(),
            });

            return new GetApiKeyResponseDto { ApiKey = key };
        }
    }
}
