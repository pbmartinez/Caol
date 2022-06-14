using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOsDailyReport : Entity
    {
        public int CoDaily { get; set; }
        public int? CoOs { get; set; }
        public int? CoFase { get; set; }
        public string CoUsuario { get; set; }
        public string DsAssunto { get; set; }
        public TimeOnly? TempoGasto { get; set; }
        public DateTime? Data { get; set; }
        public int? CoStatusAtual { get; set; }
        public int? CoChamado { get; set; }
        public int? CoAtividade { get; set; }
    }
}
