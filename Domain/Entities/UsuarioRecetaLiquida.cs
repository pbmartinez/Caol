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
    public class UsuarioRecetaLiquida
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
        [Column("yearmonth")]
        public string Yearmonth { get; set; }
    }
}
