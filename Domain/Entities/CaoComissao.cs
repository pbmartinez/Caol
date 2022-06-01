using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoComissao : Entity
    {
        public int CoComissao { get; set; }
        public int CoFatura { get; set; }
        public DateOnly DtEfetuado { get; set; }
    }
}
