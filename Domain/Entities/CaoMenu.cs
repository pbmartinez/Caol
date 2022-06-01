using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoMenu : Entity
    {
        public byte CoMenu { get; set; }
        public string DsMenu { get; set; } = null!;
        public string DsPagina { get; set; } = null!;
        public string? DsImagem { get; set; }
    }
}
