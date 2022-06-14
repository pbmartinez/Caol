using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IAppServices
{
    public partial interface ICaoFaturaAppService : IAppService<CaoFaturaDto>
    {
        /// <summary>
        /// Get performance metrics for specified users in a period of time
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="coUsuarios"></param>
        /// <returns></returns>
        List<UsuarioDto> GetRelatorio(DateTime? startDate, DateTime? endDate, IEnumerable<string> coUsuarios);

        /// <summary>
        /// Get contributions from vendors to the total sales
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="coUsuarios"></param>
        /// <returns></returns>
        AporteRecetaLiquidaDto GetPizza(DateTime? startDate, DateTime? endDate, IEnumerable<string> coUsuarios);

        /// <summary>
        /// Get contributions from vendors vs their salary
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="coUsuarios"></param>
        /// <returns></returns>
        AporteMensualDto GetGraphic(DateTime? startDate, DateTime? endDate, IEnumerable<string> coUsuarios);

        List<UsuarioDto> GetGraphicList(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios);
    }
}
