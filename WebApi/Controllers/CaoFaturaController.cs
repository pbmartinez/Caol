using Application.Constants;
using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Domain.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApi.Helpers;
using WebApi.Parameters;

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

        [HttpGet("relatorio")]
        public async Task<IActionResult> GetRelatorio([FromQuery] QueryStringParameters queryStringParameters, DateTime? startDate,DateTime? endDate, [ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<string>coUsuarios)
        {
            var facturas = await ((ICaoFaturaAppService)AppService)
                .GetRelatorioAsync(startDate,endDate,coUsuarios);
            var items = facturas.ShapeDataOnIEnumerable(queryStringParameters.Fields);
            return Ok(items);
        }

        [HttpGet("pizza")]
        public async Task<AporteRecetaLiquidaDto> GetPizzaAsync(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            var aportes = await ((ICaoFaturaAppService)AppService).GetPizzaAsync(startDate,endDate,coUsuarios);
            return aportes;
        }
    }
}
