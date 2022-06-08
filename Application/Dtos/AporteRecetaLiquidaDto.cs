using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public struct AporteRecetaLiquidaDto
    {
        public double Total { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Nombre del usuario, Receta Liquida Aportada, Porciento de aporte
        /// </summary>
        public List<ValorAporteDto> Valores { get; set; }
    }
}
