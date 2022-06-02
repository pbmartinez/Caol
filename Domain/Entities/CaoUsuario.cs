﻿using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoUsuario : Entity
    {
        public CaoUsuario()
        {
            CaoConhecimentos = new HashSet<CaoConhecimento>();
            CaoHistOcorrenciasOs = new HashSet<CaoHistOcorrenciasO>();
            CaoPontosConhecimentos = new HashSet<CaoPontosConhecimento>();
        }

        public override string Id => CoUsuario;

        public string CoUsuario { get; set; } = null!;
        public string NoUsuario { get; set; } = null!;
        public string DsSenha { get; set; } = null!;
        public string? CoUsuarioAutorizacao { get; set; }
        public ulong? NuMatricula { get; set; }
        public DateOnly? DtNascimento { get; set; }
        public DateOnly? DtAdmissaoEmpresa { get; set; }
        public DateOnly? DtDesligamento { get; set; }
        public DateTime? DtInclusao { get; set; }
        public DateOnly? DtExpiracao { get; set; }
        public string? NuCpf { get; set; }
        public string? NuRg { get; set; }
        public string? NoOrgaoEmissor { get; set; }
        public string? UfOrgaoEmissor { get; set; }
        public string? DsEndereco { get; set; }
        public string? NoEmail { get; set; }
        public string? NoEmailPessoal { get; set; }
        public string? NuTelefone { get; set; }
        public DateTime DtAlteracao { get; set; }
        public string? UrlFoto { get; set; }
        public string? InstantMessenger { get; set; }
        public uint? Icq { get; set; }
        public string? Msn { get; set; }
        public string? Yms { get; set; }
        public string? DsCompEnd { get; set; }
        public string? DsBairro { get; set; }
        public string? NuCep { get; set; }
        public string? NoCidade { get; set; }
        public string? UfCidade { get; set; }
        public DateOnly? DtExpedicao { get; set; }

        public virtual ICollection<CaoConhecimento> CaoConhecimentos { get; set; }
        public virtual ICollection<CaoHistOcorrenciasO> CaoHistOcorrenciasOs { get; set; }
        public virtual ICollection<CaoPontosConhecimento> CaoPontosConhecimentos { get; set; }
    }
}
