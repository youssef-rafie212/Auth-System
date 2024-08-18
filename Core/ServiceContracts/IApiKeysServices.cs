using Core.DTO.ApiKeyDtos;

namespace Core.ServiceContracts
{
    public interface IApiKeysServices
    {
        // Gets an API key for a new client
        Task<GetApiKeyResponseDto> GetApiKey(GetApiKeyRequestDto getApiKeyDto);
    }
}
