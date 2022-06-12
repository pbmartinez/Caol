using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UsuarioRecetasDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double[] Data { get; set; }

        public List<UsuarioDataDto> UsuarioData()
        {
            var a = new List<UsuarioDataDto>();
            var i = 0;
            foreach (var item in Data)
            {
                a.Add(new UsuarioDataDto { Index = i++, Name = this.Name, Value = item }) ;
            }
            return a;
        }

        public UsuarioRecetasDto()
        {
        }

        public UsuarioRecetasDto(string code, string name, double[] data)
        {
            Code = code;
            Name = name;
            Data = data;
        }
        public UsuarioRecetasDto(string code, string name, int n)
        {
            Code = code;
            Name = name;
            Data = new double[n];
        }
    }
}
