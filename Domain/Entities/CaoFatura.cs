using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class CaoFatura : Entity
    {

        //public override string Id => CoFatura + "";

        public uint CoFatura { get; set; }
        public uint CoCliente { get; set; }
        public int CoSistema { get; set; }
        public int CoOs { get; set; }
        public int NumNf { get; set; }
        public float Total { get; set; }
        public float Valor { get; set; }
        public DateTime DataEmissao { get; set; }
        public string CorpoNf { get; set; } = null!;
        public float ComissaoCn { get; set; }
        public float TotalImpInc { get; set; }

        [ForeignKey("CoOs")]
        public virtual CaoO CaoOrdenServicio { get; set; }

        [ForeignKey("CoCliente")]
        public virtual CaoCliente CaoCliente { get; set; }

        [ForeignKey("CoSistema")]
        public virtual CaoSistema CaoSistema { get; set; }


    }
}
