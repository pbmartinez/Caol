using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApi.Controllers
{
    [Route("api/caoo")]
    [ApiController]
    public class CaoOController : ApiBaseController<CaoODto>
    {    
        public CaoOController(ICaoOAppService appService, ILogger<ApiBaseController<CaoODto>> logger, IPropertyCheckerService propertyCheckerService) 
            : base(appService, logger, propertyCheckerService)
        {

        }
    }
}
