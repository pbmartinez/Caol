using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoTempImport : Entity
    {
        public ulong Codigo { get; set; }
        public string Descricao { get; set; } = null!;
    }
}
