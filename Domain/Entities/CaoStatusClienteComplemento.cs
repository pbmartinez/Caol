using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoStatusClienteComplemento : Entity
    {
        public uint CoComplementoStatus { get; set; }
        public string? DsStatus { get; set; }
        public uint? CoStatus { get; set; }
    }
}
