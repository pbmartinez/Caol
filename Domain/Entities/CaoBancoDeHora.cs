using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoBancoDeHora : Entity
    {
        public int Id { get; set; }
        public string CoUsuario { get; set; } = null!;
        public DateOnly DataCadastro { get; set; }
        public int Segundos { get; set; }
        public string Tipo { get; set; } = null!;
    }
}
