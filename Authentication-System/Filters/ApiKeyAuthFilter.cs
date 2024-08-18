using Core.Domain.Entities;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authentication_System.Filters
{
    public class ApiKeyAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly string _keyName = "API_KEY";
        private readonly IApiKeysServices _apiKeyServices;

        public ApiKeyAuthFilter(IApiKeysServices apiKeyServices)
        {
            _apiKeyServices = apiKeyServices;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool keyFound = context.HttpContext.Request.Headers.TryGetValue(_keyName, out var value);

            if (!keyFound || string.IsNullOrEmpty(value))
            {
                context.Result = new UnauthorizedResult();
            }

            try
            {
                APIKey apiKey = await _apiKeyServices.GetAPIKey(value!);

                context.HttpContext.Items["TenantId"] = apiKey.TenantId;
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
