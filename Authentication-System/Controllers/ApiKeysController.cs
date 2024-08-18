using Core.DTO.ApiKeyDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_System.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeysServices _services;

        public ApiKeysController(IApiKeysServices services)
        {
            _services = services;
        }

        [HttpGet("new-key")]
        public async Task<IActionResult> Get(GetApiKeyRequestDto getApiKeyRequestDto)
        {
            return Ok(await _services.GetApiKey(getApiKeyRequestDto));
        }
    }
}
