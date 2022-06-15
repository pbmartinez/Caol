using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoUf : Entity
    {
        public ulong CoUf { get; set; }
        public string DsUf { get; set; } = null!;
    }
}
