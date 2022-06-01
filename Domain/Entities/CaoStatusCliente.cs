using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoStatusCliente : Entity
    {
        public uint CoStatus { get; set; }
        public string DsStatus { get; set; } = null!;
    }
}
