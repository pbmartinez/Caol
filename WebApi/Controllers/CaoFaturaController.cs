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
    [Route("api/gateway")]
    public class CaoFaturaDtoController : ApiBaseController<CaoFaturaDto>
    {
        
        public CaoFaturaDtoController(ICaoFaturaAppService appService, ILogger<ApiBaseController<CaoFaturaDto>> logger, IPropertyCheckerService propertyCheckerService) 
            : base(appService, logger, propertyCheckerService)
        {
            
        }
    }
    
}
