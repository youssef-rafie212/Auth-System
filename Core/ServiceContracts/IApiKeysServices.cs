using Core.Domain.Entities;
using Core.DTO.ApiKeyDtos;

namespace Core.ServiceContracts
{
    public interface IApiKeysServices
    {
        // Generates an API key for a new client
        Task<GetApiKeyResponseDto> GetNewApiKey(GetApiKeyRequestDto getApiKeyDto);

        // Finds API key in the database 
        Task<APIKey> GetAPIKey(string key);

        Task<bool> DeactivateApiKey(DeactivateApiKeyDto deactivateApiKeyDto);
        Task<bool> ActivateApiKey(ActivateApiKeyDto activateApiKeyDto);
        Task<bool> DeleteApiKey(DeleteApiKeyDto deleteApiKeyDto);
    }
}
