using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/salarios")]
    [ApiController]
    public class CaoSalarioController : ApiBaseController<CaoSalarioDto>
    {
        public CaoSalarioController(ICaoSalarioAppService appService, ILogger<ApiBaseController<CaoSalarioDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
