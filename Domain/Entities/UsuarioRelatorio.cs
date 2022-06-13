using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Keyless]
    public class UsuarioRelatorio
    {
        [Column("co_usuario")]
        public string CoUsuario { get; set; }
        [Column("no_usuario")]
        public string NoUsuario { get; set; }
        [Column("data_emissao")]
        public DateTime DataEmissao { get; set; }
        [Column("valor")]
        public double Valor { get; set; }
        [Column("receita_liquida")]
        public double ReceitaLiquida { get; set; }
        [Column("brut_salario")]
        public double BrutSalario { get; set; }
        [Column("yearmonth")]
        public string Yearmonth { get; set; }
        [Column("comissao")]
        public double Comissao { get; set; }
        [Column("lucro")]
        public double Lucro { get; set; }
    }
}
