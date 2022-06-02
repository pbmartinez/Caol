using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoO : Entity
    {
        public CaoO()
        {
            CaoHistOcorrenciasOs = new HashSet<CaoHistOcorrenciasO>();
        }

        //public override string Id => CoOs + "";


        public int CoOs { get; set; }
        public int? NuOs { get; set; }
        public int? CoSistema { get; set; }
        public string? CoUsuario { get; set; }
        public int? CoArquitetura { get; set; }
        public string? DsOs { get; set; }
        public string? DsCaracteristica { get; set; }
        public string? DsRequisito { get; set; }
        public DateOnly? DtInicio { get; set; }
        public DateOnly? DtFim { get; set; }
        public int? CoStatus { get; set; }
        public string? DiretoriaSol { get; set; }
        public DateOnly? DtSol { get; set; }
        public string? NuTelSol { get; set; }
        public string? DddTelSol { get; set; }
        public string? NuTelSol2 { get; set; }
        public string? DddTelSol2 { get; set; }
        public string? UsuarioSol { get; set; }
        public DateOnly? DtImp { get; set; }
        public DateOnly? DtGarantia { get; set; }
        public int? CoEmail { get; set; }
        public int? CoOsProspectRel { get; set; }

        public virtual ICollection<CaoHistOcorrenciasO> CaoHistOcorrenciasOs { get; set; }
    }
}
