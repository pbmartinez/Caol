using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/brand")]
    [ApiController]
    public class BrandController : ApiBaseController<BrandDto>
    {
        public BrandController(ICaoUsuarioAppService appService, ILogger<ApiBaseController<BrandDto>> logger, IPropertyCheckerService propertyCheckerService) : base(appService, logger, propertyCheckerService)
        {
        }
    }
}
