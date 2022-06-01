using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoTipoCusto : Entity
    {
        public byte CoTipoCusto { get; set; }
        public string Descricao { get; set; } = null!;
    }
}
