using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoFormacaoIdiomaUsuario : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public int CoIdioma { get; set; }
        public int? NvLeitura { get; set; }
        public int? NvEscrita { get; set; }
        public int? NvFala { get; set; }
    }
}
