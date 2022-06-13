using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UsuarioDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double BrutSalario { get; set; } = 0.0;

        public List<FacturaAcumuladaDto> Facturas { get; set; } = new List<FacturaAcumuladaDto>();
    }
}
