using BlazorApp.WellKnownNames;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorApp.Identity
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, IConfiguration configuration)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { configuration[AppSettings.ApiBaseUrl] },
                scopes: new[] { configuration[AppSettings.ScopeGatewayApiAccess], "openid", "profile", "offline_access" });
        }
    }
}
