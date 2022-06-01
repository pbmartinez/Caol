using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoDiaryReport : Entity
    {
        public int CoDiaryReport { get; set; }
        public TimeOnly? HrGasta { get; set; }
        public int CoAtividade { get; set; }
        public string DsAssunto { get; set; } = null!;
        public long CoMovimento { get; set; }
        public ulong? NuOs { get; set; }
        public ulong? CoSistema { get; set; }
    }
}
