using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/permisos")]
    [ApiController]
    public class PermissaoSistemaController : ApiBaseController<PermissaoSistemaDto>
    {
        public PermissaoSistemaController(IPermissaoSistemaAppService appService, ILogger<ApiBaseController<PermissaoSistemaDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
