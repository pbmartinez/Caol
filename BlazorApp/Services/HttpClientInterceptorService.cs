using BlazorApp.Exceptions;
using Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;
using Toolbelt.Blazor;

namespace BlazorApp.Services
{
    public class HttpClientInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly NavigationManager _navManager;
        private readonly ISnackbar _snackbar;
        private readonly IStringLocalizer<Resource> _localizer;

        public HttpClientInterceptorService(HttpClientInterceptor interceptor, NavigationManager navManager, ISnackbar snackbar, IStringLocalizer<Resource> localizer)
        {
            _interceptor = interceptor ?? throw new ArgumentNullException(nameof(interceptor));
            _navManager = navManager ?? throw new ArgumentNullException(nameof(navManager));
            _snackbar = snackbar ?? throw new ArgumentNullException(nameof(snackbar));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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
                        message = _localizer[Resource.display_401_Description];
                        _navManager.NavigateTo("/401");
                        break;

                    case HttpStatusCode.Forbidden:
                        message = _localizer[Resource.display_403_Description];
                        _navManager.NavigateTo("/403");
                        break;

                    case HttpStatusCode.NotFound:
                        message = _localizer[Resource.display_404_Description];
                        _navManager.NavigateTo("/404");
                        break;

                    case HttpStatusCode.BadRequest:
                        _snackbar.Add(_localizer[Resource.display_BadRequest], Severity.Error);
                        message = _localizer[Resource.display_BadRequest];
                        break;
                    default:
                        message = _localizer[Resource.display_500_Description];
                        _snackbar.Add(_localizer[Resource.display_500_Description], Severity.Error);
                        _navManager.NavigateTo("/500");
                        break;
                }

                throw new HttpResponseException(message);
            }
        }

        public void DisposeEvent() => _interceptor.AfterSend -= InterceptResponse;
    }
}
