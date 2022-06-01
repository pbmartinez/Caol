using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoAtividadeConsultor : Entity
    {
        public int CoAtividade { get; set; }
        public string DsAtividade { get; set; } = null!;
        public string? Ativo { get; set; }
    }
}
