using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoAtividadeReport : Entity
    {
        public int Id { get; set; }
        public int CoCliente { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
        public string Lembrete { get; set; }
        public int CoAtividade { get; set; }
        public int CoOs { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }
        public string Status { get; set; } = null!;
        public string Tempo { get; set; }
        public string CoUsuario { get; set; } = null!;
        public DateTime DataAtiv { get; set; }
        public string Retorno { get; set; } = null!;
        public int? CoFase { get; set; }
    }
}
