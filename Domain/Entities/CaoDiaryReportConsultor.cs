using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// contem informacoes referentes a agendamento, visita, elabora
    /// </summary>
    public partial class CaoDiaryReportConsultor : Entity
    {
        public int CoDiaryReportConsultor { get; set; }
        public int CoMovimento { get; set; }
        public int CoAtividade { get; set; }
        public string DsEmpresa { get; set; } = null!;
        public string DsAssunto { get; set; } = null!;
        public DateTime DtAgendamento { get; set; }
        public DateTime? DtAgendamentoFim { get; set; }
        public float VlFechamento { get; set; }
        public long? CoCliente { get; set; }
    }
}
