using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoClienteContato : Entity
    {
        public uint CoCliente { get; set; }
        public DateOnly? DtContato { get; set; }
        public uint? FgAgendado { get; set; }
        public TimeOnly? HrIni { get; set; }
        public TimeOnly? HrEnd { get; set; }
    }
}
