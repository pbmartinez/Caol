using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class CaoUsuarioController : ApiBaseController<CaoUsuarioDto>
    {
        public CaoUsuarioController(ICaoUsuarioAppService appService, ILogger<ApiBaseController<CaoUsuarioDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
