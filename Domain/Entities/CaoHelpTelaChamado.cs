using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHelpTelaChamado : Entity
    {
        public int CoTela { get; set; }
        public int CoCliente { get; set; }
        public int CoSistema { get; set; }
        public string DsTela { get; set; } = null!;
    }
}
