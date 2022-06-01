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
    public class GatewayController : ApiBaseController<GatewayDto>
    {
        
        public GatewayController(IPermissaoSistemaAppService appService, ILogger<ApiBaseController<GatewayDto>> logger, IPropertyCheckerService propertyCheckerService) 
            : base(appService, logger, propertyCheckerService)
        {
            //Includes = new () { nameof(GatewayDto.Peripherals), nameof(GatewayDto.Brand) };
        }

        [HttpGet("{gatewayId}/validation-errors")]
        public async Task<ActionResult<List<string>>> ValidationErrors(Guid? gatewayId)
        {
            var validationErrors = new List<string>();

            if (gatewayId == null || gatewayId.Value == Guid.Empty)
            {
                validationErrors.Add("gateway id is not valid");
            }
            else
            {
                var gateway = await AppService.GetAsync(gatewayId.Value, Includes);

                if (gateway == null)
                    validationErrors.Add("gateway does not exist");
                else
                {
                    if (gateway.Peripherals.Count >= 10)
                        validationErrors.Add(string.Format(Resource.validation_MaxPeriphelsAllowed, GatewayPeripherals.MAX_PERIPHERALS_ALLOWED_PER_GATEWAY));
                }

            }

            return validationErrors;
        }
    }
    
}
