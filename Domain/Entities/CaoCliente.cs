using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class CaoCliente : Entity
    {
        public CaoCliente()
        {
            CaoFaturas = new HashSet<CaoFatura>();
        }

        //[Column("co_cliente")]
        ////[NotMapped]
        //public override string Id
        //{
        //    get;set;
        //    //get => CoCliente + "";
        //    //set => CoCliente = uint.Parse(value);
        //}

        public uint CoCliente { get; set; }
        public string? NoRazao { get; set; }
        public string? NoFantasia { get; set; }
        public string? NoContato { get; set; }
        public string? NuTelefone { get; set; }
        public string? NuRamal { get; set; }
        public string? NuCnpj { get; set; }
        public string? DsEndereco { get; set; }
        public int? NuNumero { get; set; }
        public string? DsComplemento { get; set; }
        public string NoBairro { get; set; } = null!;
        public string? NuCep { get; set; }
        public string? NoPais { get; set; }
        public long? CoRamo { get; set; }
        public long CoCidade { get; set; }
        public uint? CoStatus { get; set; }
        public string? DsSite { get; set; }
        public string? DsEmail { get; set; }
        public string? DsCargoContato { get; set; }
        public string? TpCliente { get; set; }
        public string? DsReferencia { get; set; }
        public uint? CoComplementoStatus { get; set; }
        public string? NuFax { get; set; }
        public string? Ddd2 { get; set; }
        public string? Telefone2 { get; set; }


        public virtual ICollection<CaoFatura> CaoFaturas { get; set; }
    }
}
