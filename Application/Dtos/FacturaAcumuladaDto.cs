using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{   
    public class FacturaAcumuladaDto
    {
        public DateTime Mes { get; set; }
        public double Valor { get; set; }
        public double RecetaLiquida { get; set; }
        public double Comissao { get; set; }
        public double Lucro { get; set; }
    }
}
