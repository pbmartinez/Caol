using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : ApiBaseController<ProviderDto>
    {
        public ProviderController(ICaoClienteAppService appService, ILogger<ApiBaseController<ProviderDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
