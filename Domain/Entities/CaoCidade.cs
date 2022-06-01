using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoCidade : Entity
    {
        public ulong CoCidade { get; set; }
        public string NoCidade { get; set; } = null!;
        public long CoUf { get; set; }
    }
}
