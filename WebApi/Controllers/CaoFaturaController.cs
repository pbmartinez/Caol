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

        [HttpGet("pizza")]
        public AporteRecetaLiquidaDto GetPizzaAsync(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            var aportes = ((ICaoFaturaAppService)AppService).GetPizza(startDate,endDate,coUsuarios);
            return aportes;
        }
        [HttpGet("graphic")]
        public AporteMensualDto GetGraphic(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            var aportes =  ((ICaoFaturaAppService)AppService).GetGraphic(startDate,endDate,coUsuarios);
            return aportes;
        }

        [HttpGet("relatorio")]
        public IActionResult GetRelatorio(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            var usuarios = ((ICaoFaturaAppService)AppService).GetRelatorio(startDate, endDate, coUsuarios);
            return Ok(usuarios);
        }
    }
}
