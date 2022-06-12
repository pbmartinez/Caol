using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ValorAporteDto
    {
        public string Code { get; set; }
        public string Name { get; set; }    
        public double RecetaLiquida { get; set; }
        public double Porciento { get; set; }
        public double Salario { get; set; }
    }
}
