using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgroSolutions.Property.API.Filters;

public class ValidateTokenFilter(IHttpClientFactory httpClientFactory) : IAsyncAuthorizationFilter
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string token = context.HttpContext.Request.Headers.Authorization.First()!;

        HttpClient client = _httpClientFactory.CreateClient("AuthService");
        HttpRequestMessage request = new(HttpMethod.Get, "api/v1/auth/validate-token");
        request.Headers.Add("Authorization", token);

        HttpResponseMessage response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            context.Result = new ContentResult { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }
    }
}
