using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoTipoIdiomaUsuario : Entity
    {
        public int CoIdioma { get; set; }
        public string? DsIdioma { get; set; }
    }
}
