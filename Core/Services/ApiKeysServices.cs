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

        public async Task<GetApiKeyResponseDto> GetApiKey(GetApiKeyRequestDto getApiKeyDto)
        {
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
