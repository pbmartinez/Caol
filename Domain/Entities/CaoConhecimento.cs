using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoConhecimento : Entity
    {
        public CaoConhecimento()
        {
            CaoConhecimentosFontes = new HashSet<CaoConhecimentosFonte>();
            CaoPontosConhecimentos = new HashSet<CaoPontosConhecimento>();
        }

        public int Idconhecimento { get; set; }
        public string Assunto { get; set; } = null!;
        public string Conhecimento { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Tags { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public DateTime Datahora { get; set; }

        public virtual CaoUsuario UsuarioNavigation { get; set; } = null!;
        public virtual ICollection<CaoConhecimentosFonte> CaoConhecimentosFontes { get; set; }
        public virtual ICollection<CaoPontosConhecimento> CaoPontosConhecimentos { get; set; }
    }
}
