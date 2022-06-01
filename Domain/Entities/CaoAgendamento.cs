using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoAgendamento : Entity
    {
        public ulong CoAgendamento { get; set; }
        public DateTime DtHrInicio { get; set; }
        public DateTime? DtHrFim { get; set; }
        public long CoStatusAgendamento { get; set; }
        public long CoDiaryReportConsultor { get; set; }
        public long CoComplemento { get; set; }
    }
}
