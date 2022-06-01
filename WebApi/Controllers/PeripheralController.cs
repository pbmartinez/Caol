using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("api/peripheral")]
    public class PeripheralController : ApiBaseController<PeripheralDto>
    {
        private readonly IPermissaoSistemaAppService _gatewayAppService;        
        public PeripheralController(ICaoFaturaAppService appService, ILogger<ApiBaseController<PeripheralDto>> logger, IPropertyCheckerService propertyCheckerService,
            IPermissaoSistemaAppService gatewayAppService) 
            : base(appService, logger, propertyCheckerService)
        {
            _gatewayAppService = gatewayAppService;
            Includes = new List<string>() { nameof(PeripheralDto.Gateway)};
        }

        [HttpGet]
        [HttpHead]
        [Route("~/api/gateway/{gatewayId}/peripheral/{peripheralId}")]
        public async Task<ActionResult<PeripheralDto>> GetPeripheralForGateway(Guid? gatewayId, Guid? peripheralId)
        {
            if (gatewayId == null || gatewayId == Guid.Empty)
                return BadRequest();
            if (peripheralId == null || peripheralId == Guid.Empty)
                return BadRequest();
            var gateway = await _gatewayAppService.GetAsync(gatewayId.Value, Includes);
            if (gateway == null)
                return NotFound();
            var peripheral = gateway.Peripherals.FirstOrDefault(p=> p.Id == peripheralId.Value);
            if (peripheral == null)
                return NotFound();
            return Ok(peripheral);
        }
        [HttpGet]
        [HttpHead]
        [Route("~/api/gateway/{gatewayId}/peripheral")]
        public async Task<ActionResult<PeripheralDto>> GetAllPeripheralForGateway(Guid? gatewayId)
        {
            if (gatewayId == null || gatewayId == Guid.Empty)
                return BadRequest();
            var gateway = await _gatewayAppService.GetAsync(gatewayId.Value, Includes);
            if (gateway == null)
                return NotFound();
            return Ok(gateway.Peripherals);
        }

        [HttpPost]
        [Route("~/api/gateway/{gatewayId}/peripheral")]
        public async Task<ActionResult> PostPeripheralForGateway(Guid? gatewayId, PeripheralDto peripheral)
        {
            if (gatewayId == null || gatewayId == Guid.Empty)
                return BadRequest();
            var gateway = await _gatewayAppService.GetAsync(gatewayId.Value);
            if(gateway == null)
                return BadRequest();
            peripheral.Id = Guid.NewGuid();
            peripheral.GatewayId = gatewayId.Value;
            var result = await AppService.AddAsync(peripheral);
            var peripheralDto = await AppService.GetAsync(peripheral.Id);
            return CreatedAtAction(nameof(GetPeripheralForGateway), new { gatewayId = gatewayId.Value, peripheralId = peripheral.Id }, peripheralDto);
        }

        [HttpPut]
        [Route("~/api/gateway/{gatewayId}/peripheral")]
        public async Task<ActionResult> PutPeripheralForGateway(Guid? gatewayId, PeripheralDto peripheral)
        {
            if (gatewayId == null || gatewayId == Guid.Empty)
                return BadRequest();
            var gateway = await _gatewayAppService.GetAsync(gatewayId.Value, Includes);
            if (gateway == null)
                return NotFound();
            var peripheralTarget = gateway.Peripherals.FirstOrDefault(p => p.Id == peripheral.Id);
            if (peripheralTarget == null)
                return BadRequest();
            var result = await AppService.UpdateAsync(peripheral);
            return NoContent();
        }
                
        [HttpPatch]
        [Route("~/api/gateway/{gatewayId}/peripheral/{peripheralId}")]
        public async Task<ActionResult> PatchPeripheralForGateway(Guid? gatewayId,Guid? peripheralId, JsonPatchDocument<PeripheralDto> patchDocument)
        {
            if (gatewayId == null || gatewayId == Guid.Empty || peripheralId == null || peripheralId == Guid.Empty)
                return BadRequest();
            var gateway = await _gatewayAppService.GetAsync(gatewayId.Value, Includes);
            if (gateway == null)
                return NotFound();
            var peripheralTarget = gateway.Peripherals.FirstOrDefault(p => p.Id == peripheralId.Value);
            if (peripheralTarget == null)
                return BadRequest();
            patchDocument.ApplyTo(peripheralTarget);
            if (!TryValidateModel(peripheralTarget))
                return ValidationProblem(ModelState);
            var result = await AppService.UpdateAsync(peripheralTarget);
            return NoContent();
        }

        [HttpDelete]
        [Route("~/api/gateway/{gatewayId}/peripheral/{peripheralId}")]
        public async Task<ActionResult> DeletePeripheralForGateway(Guid? gatewayId,Guid? peripheralId)
        {
            if (gatewayId == null || gatewayId == Guid.Empty || peripheralId == null || peripheralId == Guid.Empty)
                return BadRequest();
            var gateway = await _gatewayAppService.GetAsync(gatewayId.Value, Includes);
            if (gateway == null)
                return NotFound();
            var peripheralTarget = gateway.Peripherals.FirstOrDefault(p => p.Id == peripheralId.Value);
            if (peripheralTarget == null)
                return BadRequest();
            var result = await AppService.RemoveAsync(peripheralTarget.Id);
            return NoContent();
        }
    }
}
