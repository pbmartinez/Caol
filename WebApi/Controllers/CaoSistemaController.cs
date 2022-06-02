using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/caosistema")]
    [ApiController]
    public class CaoSistemaController : ApiBaseController<CaoSistemaDto>
    {
        public CaoSistemaController(ICaoSistemaAppService appService, ILogger<ApiBaseController<CaoSistemaDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
