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
        public ActionResult<AporteRecetaLiquidaDto> GetPizzaAsync(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            if (startDate == null || endDate == null || coUsuarios == null || !coUsuarios.Any() || startDate > endDate)
                return BadRequest();

            var aportes = ((ICaoFaturaAppService)AppService).GetPizza(startDate, endDate, coUsuarios);
            return Ok(aportes);
        }
        [HttpGet("graphic")]
        public ActionResult<AporteMensualDto> GetGraphic(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            if (startDate == null || endDate == null || coUsuarios == null || !coUsuarios.Any() || startDate > endDate)
                return BadRequest();
            var aportes = ((ICaoFaturaAppService)AppService).GetGraphic(startDate, endDate, coUsuarios);
            return Ok(aportes);
        }
        [HttpGet("graphic2")]
        public ActionResult<List<UsuarioDto>> GetGraphic2(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            if (startDate == null || endDate == null || coUsuarios == null || !coUsuarios.Any() || startDate > endDate)
                return BadRequest();
            var aportes = ((ICaoFaturaAppService)AppService).GetGraphicList(startDate, endDate, coUsuarios);
            return Ok(aportes);
        }

        [HttpGet("relatorio")]
        public ActionResult<List<UsuarioDto>> GetRelatorio(DateTime? startDate, DateTime? endDate, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> coUsuarios)
        {
            if (startDate == null || endDate == null || coUsuarios == null || !coUsuarios.Any() || startDate > endDate)
                return BadRequest();
            var usuarios = ((ICaoFaturaAppService)AppService).GetRelatorio(startDate, endDate, coUsuarios);
            return Ok(usuarios);
        }
    }
}
