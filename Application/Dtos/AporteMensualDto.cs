using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class AporteMensualDto
    {
        public double AvarageSalary { get; set; }
        public List<DateTime> Months { get; set; } = new List<DateTime>();
        public List<UsuarioRecetasDto> UsuarioRecetas { get; set; } = new List<UsuarioRecetasDto>();
    }
}
