using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/brand")]
    [ApiController]
    public class CaoClienteController : ApiBaseController<CaoClienteDto>
    {
        public CaoClienteController(ICaoClienteAppService appService, ILogger<ApiBaseController<CaoClienteDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
