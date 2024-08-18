using Core.Domain.Entities;
using Core.Domain.RepositoryContracts;
using Core.DTO.ApiKeyDtos;
using Core.Helpers;
using Core.ServiceContracts;

namespace Core.Services
{
    public class ApiKeysServices : IApiKeysServices
    {
        private IApiKeysRepository _repo;

        public ApiKeysServices(IApiKeysRepository repo)
        {
            _repo = repo;
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
                Key = ServicesHelpers.GenerateUniqueString(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                TenantId = Guid.NewGuid(),
            });

            return new GetApiKeyResponseDto { ApiKey = key };
        }
    }
}
