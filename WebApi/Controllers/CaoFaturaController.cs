using Application.Constants;
using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Domain.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/facturas")]
    public class CaoFaturaController : ApiBaseController<CaoFaturaDto>
    {
        public CaoFaturaController(ICaoFaturaAppService appService, ILogger<ApiBaseController<CaoFaturaDto>> logger, IPropertyCheckerService propertyCheckerService) 
            : base(appService, logger, propertyCheckerService)
        {
            
        }
    }
    
}
