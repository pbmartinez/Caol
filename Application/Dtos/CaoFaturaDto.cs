using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Dtos
{
    public partial class CaoFaturaDto : Entity
    {
        public uint CoFatura { get; set; }
        public int CoCliente { get; set; }
        public int CoSistema { get; set; }
        public int CoOs { get; set; }
        public int NumNf { get; set; }
        public float Total { get; set; }
        public float Valor { get; set; }
        public DateOnly DataEmissao { get; set; }
        public string CorpoNf { get; set; } = null!;
        public float ComissaoCn { get; set; }
        public float TotalImpInc { get; set; }



        public virtual CaoODto CaoOrdenServicio { get; set; }

        public virtual CaoClienteDto CaoCliente { get; set; }

        public virtual CaoSistemaDto CaoSistema { get; set; }



        /// <summary>
        /// Receita Liquida o valor líquido
        /// </summary>
        public float ReceitaLiquida => Valor - (TotalImpInc % 100 * Valor);

        /// <summary>
        /// Valor de Comision
        /// </summary>
        public float Comissao => (Valor - (TotalImpInc % 100 * Valor)) * (ComissaoCn % 100);

        /// <summary>
        /// Lucro. Ganancia neta de la empresa 
        /// </summary>
        public float Lucro => ReceitaLiquida - Comissao - CaoOrdenServicio.CaoUsuario.CaoSalario.BrutSalario;

    }
}
