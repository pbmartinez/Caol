using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoAtividade : Entity
    {
        public int CoAtividade { get; set; }
        public string DsAtividade { get; set; } = null!;
        public long CoTipoUsuario { get; set; }
    }
}
