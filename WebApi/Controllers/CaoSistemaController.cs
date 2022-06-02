using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/CaoSistema")]
    [ApiController]
    public class CaoSistemaController : ApiBaseController<CaoSistemaDto>
    {
        public CaoSistemaController(ICaoClienteAppService appService, ILogger<ApiBaseController<CaoSistemaDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
