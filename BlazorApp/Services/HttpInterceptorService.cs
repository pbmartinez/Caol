using BlazorApp.Exceptions;
using Microsoft.AspNetCore.Components;
using System.Net;
using Toolbelt.Blazor;

namespace BlazorApp.Services
{
    public class HttpInterceptorService 
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly NavigationManager _navManager;

        public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navManager)
        {
            _interceptor = interceptor ?? throw new ArgumentNullException(nameof(interceptor));
            _navManager = navManager ?? throw new ArgumentNullException(nameof(navManager));
        }

        public void RegisterEvent() => _interceptor.AfterSend += InterceptResponse;

        private void InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
        {
            if (!e.Response.IsSuccessStatusCode)
            {
                var message = string.Empty;
                var statusCode = e.Response.StatusCode;

                switch (statusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        _navManager.NavigateTo("/401");
                        message = "User is not authorized";
                        break;
                    case HttpStatusCode.Forbidden:
                        _navManager.NavigateTo("/403");
                        message = "User is not authorized";
                        break;
                    case HttpStatusCode.NotFound:
                        _navManager.NavigateTo("/404");
                        message = "The requested resorce was not found.";
                        break;
                                    
                    default:
                        _navManager.NavigateTo("/500");
                        message = "Something went wrong, please contact Administrator";
                        break;
                }

                throw new HttpResponseException(message);
            }
        }

        public void DisposeEvent() => _interceptor.AfterSend -= InterceptResponse;
    }
}
