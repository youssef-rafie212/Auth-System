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
            try
            {
                return Ok(await _services.GetNewApiKey(getApiKeyRequestDto));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }
    }
}
