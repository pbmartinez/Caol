using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoStatusO : Entity
    {
        public uint CoStatusAtual { get; set; }
        public string DsStatus { get; set; } = null!;
    }
}
