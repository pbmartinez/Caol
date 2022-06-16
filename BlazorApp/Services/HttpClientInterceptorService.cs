using BlazorApp.Exceptions;
using Microsoft.AspNetCore.Components;
using System.Net;
using Toolbelt.Blazor;

namespace BlazorApp.Services
{
    public class HttpClientInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly NavigationManager _navManager;

        public HttpClientInterceptorService(HttpClientInterceptor interceptor, NavigationManager navManager)
        {
            _interceptor = interceptor;
            _navManager = navManager;
        }

        public void RegisterEvent() => _interceptor.AfterSend += InterceptResponse;

        private void InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
        {
            string message = string.Empty;
            
            if (!e.Response.IsSuccessStatusCode)
            {
                var statusCode = e.Response.StatusCode;

                switch (statusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        message = "You are not authenticated";
                        _navManager.NavigateTo("/401");
                        break;

                    case HttpStatusCode.Forbidden:
                        message = "You are not allowed";
                        _navManager.NavigateTo("/403");
                        break;

                    case HttpStatusCode.NotFound:
                        message = "Requested resource is not found";
                        _navManager.NavigateTo("/404");
                        break;

                    case HttpStatusCode.BadRequest:
                        message = "Problem with the request";
                        break;
                    default:
                        message = "Internal server error";
                        _navManager.NavigateTo("/500");
                        break;
                }

                throw new HttpResponseException(message);
            }
        }

        public void DisposeEvent() => _interceptor.AfterSend -= InterceptResponse;
    }
}
