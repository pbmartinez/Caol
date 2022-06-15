using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoAviso : Entity
    {
        public uint CoAviso { get; set; }
        public string DsAviso { get; set; } = null!;
    }
}
