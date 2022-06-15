using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoMovimentoJustificativa : Entity
    {
        public ulong CoMovimentoJustificativa { get; set; }
        public ulong CoMovimento { get; set; }
        public ulong NuOs { get; set; }
        public string DsJustificativa { get; set; } = null!;
    }
}
