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

        [HttpGet("new")]
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Activate(ActivateApiKeyDto activateApiKeyDto)
        {
            bool success = await _services.ActivateApiKey(activateApiKeyDto);

            if (success) return Ok("Key activated.");

            return Problem("Key failed to activate please make sure its a valid key and that its not already active.", statusCode: 400);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Deactivate(DeactivateApiKeyDto deactivateApiKeyDto)
        {
            bool success = await _services.DeactivateApiKey(deactivateApiKeyDto);

            if (success) return Ok("Key deactivated.");

            return Problem("Key failed to deactivate please make sure its a valid key and that its active.", statusCode: 400);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteApiKeyDto deleteApiKeyDto)
        {
            bool success = await _services.DeleteApiKey(deleteApiKeyDto);

            if (success) return Ok("Key deleted.");

            return Problem("Key failed to be deleted please make sure its a valid key.", statusCode: 400);
        }
    }
}
