using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class AporteRecetaLiquidaDto
    {
        public AporteRecetaLiquidaDto()
        {
            Valores = new List<(string, double, double)>();
        }

        public double Total { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Nombre del usuario, Receta Liquida Aportada, Porciento de aporte
        /// </summary>
        public List<(string Name,double RecetaLiquida,double Porciento)> Valores { get; set; }
    }
}
