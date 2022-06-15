using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoStatusAgendamento : Entity
    {
        public ulong CoStatusAgendamento { get; set; }
        public string DsStatusAgendamento { get; set; } = null!;
    }
}
