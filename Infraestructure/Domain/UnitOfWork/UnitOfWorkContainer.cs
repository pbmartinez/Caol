using Domain.Entities;
using Domain.Utils;
using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infraestructure.Domain.UnitOfWork
{
    public partial class UnitOfWorkContainer : BaseDbContext, IUnitOfWork
    {
        public UnitOfWorkContainer(DbContextOptions<UnitOfWorkContainer> options) : base(options)
        {
            
        }

        public async Task<int> CommitAsync()
        {
            return await SaveChangesAsync();
        }
        //Todo : Copiar un metodo como este y hacerlo que reciba la consulta para pasarsela al dbset
        public DbSet<TEntity> Repository<TEntity>() where TEntity : Entity
        {
            return Set<TEntity>();
        }        

        public async Task RollbackAsync()
        {
            await RollbackAsync();
        }

        public void SetEntryState<TEntity>(TEntity item, EntityState state) where TEntity : class
        {
            Entry(item).State = state;
        }

        public EntityEntry GetEntry<TEntity>(TEntity item) where TEntity : class
        {
            return Entry(item);
        }

        ChangeTracker IUnitOfWork.ChangeTracker()
        {
            return ChangeTracker;
        }

        
        public IQueryable<TEntity> GetQueryable<TEntity>(List<string>? includes = null, Expression<Func<TEntity, bool>>? predicate = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0) where TEntity : class
        {
            IQueryable<TEntity> items = Set<TEntity>();
            if (includes != null && includes.Any())
                includes.Where(i => !string.IsNullOrEmpty(i) && !string.IsNullOrWhiteSpace(i)).ToList()
                    .ForEach(a => items = items.Include(a));

            if (predicate != null)
                items = items.Where(predicate);

            if (order != null && order.Any())
            {
                order.Where(i => i.Key != null).ToList()
                    .ForEach(s => items = QueryableUtils.CallOrderBy(items, s.Key, s.Value));
                if (pageSize > 0)
                {
                    var skip = pageSize * (pageGo - 1);
                    skip = skip >= 0 ? skip : 0;
                    items = items.Skip(skip);
                }
            }
            if (pageSize > 0)
            {
                items = items.Take(pageSize);
            }

            return items;
        }
        public IQueryable<T> ExecuteQuery<T>(string query, params object[] parameters) where T : class
        {
            var res = Set<T>().FromSqlRaw<T>(query, parameters).ToList();
            return Set<T>().FromSqlRaw<T>(query,parameters).AsQueryable();
        }

        //public IQueryable<TEntity> ExecuteQuery2<TEntity>(string query, params object[] parameters) where TEntity : class
        //{
        //    return Set<TEntity>().FromSqlRaw<TEntity>(query,parameters).AsQueryable();
        //}
        

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sqlCommand, parameters);
        }

        public async Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }

        /// <summary>
        /// Used for mapping objects to custom raw sql queries 
        /// </summary>
        #region Database Raw Query Results
        public DbSet<UsuarioRecetaLiquida> UsuarioRecetaLiquida { get; set; }
        public DbSet<UsuarioPerformance> UsuarioPerformance { get; set; }
        public DbSet<UsuarioRelatorio> UsuarioRelatorio { get; set; }
        #endregion


        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_spanish_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<CaoAcompanhamentoSistema>(entity =>
            {
                entity.HasKey(e => e.CoAcompanhamento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_acompanhamento_sistema");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAcompanhamento).HasColumnName("co_acompanhamento");

                entity.Property(e => e.CoSistema)
                    .HasColumnName("co_sistema")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Email)
                    .HasMaxLength(40)
                    .HasColumnName("email");

                entity.Property(e => e.Senha)
                    .HasMaxLength(20)
                    .HasColumnName("senha");

                entity.Property(e => e.Status)
                    .HasMaxLength(2)
                    .HasColumnName("status")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CaoAgendamento>(entity =>
            {
                entity.HasKey(e => e.CoAgendamento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_agendamento");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAgendamento).HasColumnName("co_agendamento");

                entity.Property(e => e.CoComplemento).HasColumnName("co_complemento");

                entity.Property(e => e.CoDiaryReportConsultor).HasColumnName("co_diary_report_consultor");

                entity.Property(e => e.CoStatusAgendamento).HasColumnName("co_status_agendamento");

                entity.Property(e => e.DtHrFim)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_hr_fim");

                entity.Property(e => e.DtHrInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_hr_inicio")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");
            });

            modelBuilder.Entity<CaoArquiteturaO>(entity =>
            {
                entity.HasKey(e => e.CoArquitetura)
                    .HasName("PRIMARY");

                entity.ToTable("cao_arquitetura_os");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoArquitetura)
                    .ValueGeneratedNever()
                    .HasColumnName("co_arquitetura");

                entity.Property(e => e.DsArquitetura)
                    .HasMaxLength(20)
                    .HasColumnName("ds_arquitetura")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoAtividade>(entity =>
            {
                entity.HasKey(e => e.CoAtividade)
                    .HasName("PRIMARY");

                entity.ToTable("cao_atividade");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.CoTipoUsuario).HasColumnName("co_tipo_usuario");

                entity.Property(e => e.DsAtividade)
                    .HasMaxLength(30)
                    .HasColumnName("ds_atividade")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoAtividadeConsultor>(entity =>
            {
                entity.HasKey(e => e.CoAtividade)
                    .HasName("PRIMARY");

                entity.ToTable("cao_atividade_consultor");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.Ativo)
                    .HasMaxLength(1)
                    .HasColumnName("ativo")
                    .IsFixedLength();

                entity.Property(e => e.DsAtividade)
                    .HasMaxLength(50)
                    .HasColumnName("ds_atividade")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoAtividadeReport>(entity =>
            {
                entity.ToTable("cao_atividade_report");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Assunto)
                    .HasMaxLength(60)
                    .HasColumnName("assunto");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoFase)
                    .HasColumnName("co_fase")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoOs).HasColumnName("co_os");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Conteudo)
                    .HasColumnType("text")
                    .HasColumnName("conteudo");

                entity.Property(e => e.DataAtiv)
                    .HasColumnType("datetime")
                    .HasColumnName("data_ativ")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Fim)
                    .HasMaxLength(30)
                    .HasColumnName("fim");

                entity.Property(e => e.Inicio)
                    .HasMaxLength(30)
                    .HasColumnName("inicio");

                entity.Property(e => e.Lembrete)
                    .HasMaxLength(50)
                    .HasColumnName("lembrete");

                entity.Property(e => e.Retorno)
                    .HasColumnType("text")
                    .HasColumnName("retorno");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tempo)
                    .HasMaxLength(30)
                    .HasColumnName("tempo");
            });

            modelBuilder.Entity<CaoAtividadeTeste>(entity =>
            {
                entity.HasKey(e => e.CoAtividade)
                    .HasName("PRIMARY");

                entity.ToTable("cao_atividade_teste");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.CoTipoUsuario).HasColumnName("co_tipo_usuario");

                entity.Property(e => e.DsAtividade)
                    .HasMaxLength(30)
                    .HasColumnName("ds_atividade")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoAviso>(entity =>
            {
                entity.HasKey(e => e.CoAviso)
                    .HasName("PRIMARY");

                entity.ToTable("cao_aviso");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAviso).HasColumnName("co_aviso");

                entity.Property(e => e.DsAviso)
                    .HasColumnType("text")
                    .HasColumnName("ds_aviso");
            });

            modelBuilder.Entity<CaoBancoDeHora>(entity =>
            {
                entity.ToTable("cao_banco_de_horas");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("data_cadastro")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.Segundos).HasColumnName("segundos");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(10)
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");
            });

            modelBuilder.Entity<CaoBancoHora>(entity =>
            {
                entity.HasKey(e => e.CoBancHoras)
                    .HasName("PRIMARY");

                entity.ToTable("cao_banco_horas");

                entity.HasComment("Banco de horas dos usuários...")
                    .HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoBancHoras).HasColumnName("co_banc_horas");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.MinFerias).HasColumnName("min_ferias");

                entity.Property(e => e.MinMes).HasColumnName("min_mes");

                entity.Property(e => e.MinPago).HasColumnName("min_pago");

                entity.Property(e => e.MinTotal).HasColumnName("min_total");

                entity.Property(e => e.Periodo)
                    .HasMaxLength(7)
                    .HasColumnName("periodo")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoBoleto>(entity =>
            {
                entity.HasKey(e => e.CoBoleto)
                    .HasName("PRIMARY");

                entity.ToTable("cao_boleto");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoBoleto).HasColumnName("co_boleto");

                entity.Property(e => e.Boleto)
                    .HasMaxLength(255)
                    .HasColumnName("boleto");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoOs).HasColumnName("co_os");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.LinhaDig)
                    .HasMaxLength(255)
                    .HasColumnName("linha_dig");

                entity.Property(e => e.Parcela)
                    .HasMaxLength(20)
                    .HasColumnName("parcela");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Valor)
                    .HasMaxLength(128)
                    .HasColumnName("valor")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Vencimento)
                    .HasMaxLength(128)
                    .HasColumnName("vencimento")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoBonu>(entity =>
            {
                entity.HasKey(e => new { e.BonCategoria, e.BonInicio, e.BonFim })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("cao_bonus");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.BonCategoria).HasColumnName("bon_categoria");

                entity.Property(e => e.BonInicio).HasColumnName("bon_inicio");

                entity.Property(e => e.BonFim).HasColumnName("bon_fim");

                entity.Property(e => e.BonValorFimsem).HasColumnName("bon_valor_fimsem");

                entity.Property(e => e.BonValorSem).HasColumnName("bon_valor_sem");
            });

            modelBuilder.Entity<CaoBonusStatus>(entity =>
            {
                entity.ToTable("cao_bonus_status");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(128)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DataSolic)
                    .HasColumnName("data_solic")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.IsHoras)
                    .HasMaxLength(10)
                    .HasColumnName("is_horas");

                entity.Property(e => e.IsPg)
                    .HasMaxLength(10)
                    .HasColumnName("is_pg");

                entity.Property(e => e.IsSolic)
                    .HasMaxLength(10)
                    .HasColumnName("is_solic");

                entity.Property(e => e.Mes)
                    .HasMaxLength(128)
                    .HasColumnName("mes")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Valor)
                    .HasMaxLength(128)
                    .HasColumnName("valor")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoCategoriasOmbudsman>(entity =>
            {
                entity.HasKey(e => e.Idcategoria)
                    .HasName("PRIMARY");

                entity.ToTable("cao_categorias_ombudsman");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(100)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoCidade>(entity =>
            {
                entity.HasKey(e => e.CoCidade)
                    .HasName("PRIMARY");

                entity.ToTable("cao_cidade");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoCidade).HasColumnName("co_cidade");

                entity.Property(e => e.CoUf).HasColumnName("co_uf");

                entity.Property(e => e.NoCidade)
                    .HasMaxLength(30)
                    .HasColumnName("no_cidade")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoCliente>(entity =>
            {
                entity.HasKey(e => e.CoCliente)
                    .HasName("PRIMARY");

                entity.ToTable("cao_cliente");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");


                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoCidade).HasColumnName("co_cidade");

                entity.Property(e => e.CoComplementoStatus).HasColumnName("co_complemento_status");

                entity.Property(e => e.CoRamo).HasColumnName("co_ramo");

                entity.Property(e => e.CoStatus).HasColumnName("co_status");

                entity.Property(e => e.Ddd2)
                    .HasMaxLength(10)
                    .HasColumnName("ddd2");

                entity.Property(e => e.DsCargoContato)
                    .HasMaxLength(50)
                    .HasColumnName("ds_cargo_contato");

                entity.Property(e => e.DsComplemento)
                    .HasMaxLength(150)
                    .HasColumnName("ds_complemento");

                entity.Property(e => e.DsEmail)
                    .HasMaxLength(50)
                    .HasColumnName("ds_email");

                entity.Property(e => e.DsEndereco)
                    .HasMaxLength(150)
                    .HasColumnName("ds_endereco");

                entity.Property(e => e.DsReferencia)
                    .HasMaxLength(100)
                    .HasColumnName("ds_referencia");

                entity.Property(e => e.DsSite)
                    .HasMaxLength(50)
                    .HasColumnName("ds_site");

                entity.Property(e => e.NoBairro)
                    .HasMaxLength(50)
                    .HasColumnName("no_bairro")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.NoContato)
                    .HasMaxLength(30)
                    .HasColumnName("no_contato");

                entity.Property(e => e.NoFantasia)
                    .HasMaxLength(50)
                    .HasColumnName("no_fantasia");

                entity.Property(e => e.NoPais)
                    .HasMaxLength(50)
                    .HasColumnName("no_pais");

                entity.Property(e => e.NoRazao)
                    .HasMaxLength(50)
                    .HasColumnName("no_razao");

                entity.Property(e => e.NuCep)
                    .HasMaxLength(10)
                    .HasColumnName("nu_cep");

                entity.Property(e => e.NuCnpj)
                    .HasMaxLength(18)
                    .HasColumnName("nu_cnpj");

                entity.Property(e => e.NuFax)
                    .HasMaxLength(15)
                    .HasColumnName("nu_fax");

                entity.Property(e => e.NuNumero).HasColumnName("nu_numero");

                entity.Property(e => e.NuRamal)
                    .HasMaxLength(6)
                    .HasColumnName("nu_ramal");

                entity.Property(e => e.NuTelefone)
                    .HasMaxLength(15)
                    .HasColumnName("nu_telefone");

                entity.Property(e => e.Telefone2)
                    .HasMaxLength(20)
                    .HasColumnName("telefone2");

                entity.Property(e => e.TpCliente)
                    .HasMaxLength(2)
                    .HasColumnName("tp_cliente")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CaoClienteContato>(entity =>
            {
                entity.HasKey(e => e.CoCliente)
                    .HasName("PRIMARY");

                entity.ToTable("cao_cliente_contato");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoCliente)
                    .ValueGeneratedNever()
                    .HasColumnName("co_cliente");

                entity.Property(e => e.DtContato)
                    .HasColumnName("dt_contato")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.FgAgendado)
                    .HasColumnName("fg_agendado")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.HrEnd)
                    .HasColumnType("time")
                    .HasColumnName("hr_end")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.HrIni)
                    .HasColumnType("time")
                    .HasColumnName("hr_ini")
                    .HasDefaultValueSql("'00:00:00'");
            });

            modelBuilder.Entity<CaoComissao>(entity =>
            {
                entity.HasKey(e => e.CoComissao)
                    .HasName("PRIMARY");

                entity.ToTable("cao_comissao");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoFatura, "co_fatura")
                    .IsUnique();

                entity.Property(e => e.CoComissao).HasColumnName("co_comissao");

                entity.Property(e => e.CoFatura).HasColumnName("co_fatura");

                entity.Property(e => e.DtEfetuado)
                    .HasColumnName("dt_efetuado")
                    .HasDefaultValueSql("'0000-00-00'");
            });

            modelBuilder.Entity<CaoComplemento>(entity =>
            {
                entity.HasKey(e => e.CoComplemento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_complemento");

                entity.HasComment("Descrição da Visita")
                    .HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoComplemento).HasColumnName("co_complemento");

                entity.Property(e => e.DsComplemento).HasColumnName("ds_complemento");
            });

            modelBuilder.Entity<CaoConhecimento>(entity =>
            {
                entity.HasKey(e => e.Idconhecimento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_conhecimentos");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.Usuario, "usuario");

                entity.Property(e => e.Idconhecimento).HasColumnName("idconhecimento");

                entity.Property(e => e.Assunto)
                    .HasMaxLength(100)
                    .HasColumnName("assunto")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Conhecimento)
                    .HasColumnType("text")
                    .HasColumnName("conhecimento");

                entity.Property(e => e.Datahora)
                    .HasColumnType("datetime")
                    .HasColumnName("datahora")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Tags)
                    .HasColumnType("text")
                    .HasColumnName("tags");

                entity.Property(e => e.Url)
                    .HasColumnType("tinytext")
                    .HasColumnName("url");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(20)
                    .HasColumnName("usuario")
                    .HasDefaultValueSql("''");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.CaoConhecimentos)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_conhecimentos_ibfk_1");
            });

            modelBuilder.Entity<CaoConhecimentoUsuario>(entity =>
            {
                entity.HasKey(e => new { e.CoUsuario, e.CoConhecimento })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("cao_conhecimento_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoConhecimento).HasColumnName("co_conhecimento");

                entity.Property(e => e.IsCertificado).HasColumnName("is_certificado");

                entity.Property(e => e.NvConhecimento).HasColumnName("nv_conhecimento");
            });

            modelBuilder.Entity<CaoConhecimentosFonte>(entity =>
            {
                entity.HasKey(e => e.Idfonte)
                    .HasName("PRIMARY");

                entity.ToTable("cao_conhecimentos_fontes");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.Idconhecimento, "idconhecimento");

                entity.Property(e => e.Idfonte).HasColumnName("idfonte");

                entity.Property(e => e.Arquivo)
                    .HasMaxLength(50)
                    .HasColumnName("arquivo")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Caminho)
                    .HasMaxLength(50)
                    .HasColumnName("caminho")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Datahora)
                    .HasColumnType("datetime")
                    .HasColumnName("datahora")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Idconhecimento).HasColumnName("idconhecimento");

                entity.HasOne(d => d.IdconhecimentoNavigation)
                    .WithMany(p => p.CaoConhecimentosFontes)
                    .HasForeignKey(d => d.Idconhecimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_conhecimentos_fontes_ibfk_1");
            });

            modelBuilder.Entity<CaoCusto>(entity =>
            {
                entity.HasKey(e => e.CoCusto)
                    .HasName("PRIMARY");

                entity.ToTable("cao_custo");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoCusto).HasColumnName("co_custo");

                entity.Property(e => e.CoBoleto)
                    .HasColumnType("text")
                    .HasColumnName("co_boleto");

                entity.Property(e => e.CoCustoHigh).HasColumnName("co_custo_high");

                entity.Property(e => e.CoEscritorio).HasColumnName("co_escritorio");

                entity.Property(e => e.CoTipoCusto).HasColumnName("co_tipo_custo");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(100)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DtCompra).HasColumnName("dt_compra");

                entity.Property(e => e.DtPagamento).HasColumnName("dt_pagamento");

                entity.Property(e => e.Pag).HasColumnName("pag");

                entity.Property(e => e.Parcela)
                    .HasMaxLength(5)
                    .HasColumnName("parcela");

                entity.Property(e => e.Valor).HasColumnName("valor");
            });

            modelBuilder.Entity<CaoDiaryReport>(entity =>
            {
                entity.HasKey(e => e.CoDiaryReport)
                    .HasName("PRIMARY");

                entity.ToTable("cao_diary_report");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoDiaryReport).HasColumnName("co_diary_report");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.CoMovimento).HasColumnName("co_movimento");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.DsAssunto).HasColumnName("ds_assunto");

                entity.Property(e => e.HrGasta)
                    .HasColumnType("time")
                    .HasColumnName("hr_gasta")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.NuOs).HasColumnName("nu_os");
            });

            modelBuilder.Entity<CaoDiaryReportConsultor>(entity =>
            {
                entity.HasKey(e => e.CoDiaryReportConsultor)
                    .HasName("PRIMARY");

                entity.ToTable("cao_diary_report_consultor");

                entity.HasComment("contem informacoes referentes a agendamento, visita, elabora")
                    .HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoDiaryReportConsultor).HasColumnName("co_diary_report_consultor");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoMovimento).HasColumnName("co_movimento");

                entity.Property(e => e.DsAssunto)
                    .HasColumnType("mediumtext")
                    .HasColumnName("ds_assunto");

                entity.Property(e => e.DsEmpresa)
                    .HasColumnType("mediumtext")
                    .HasColumnName("ds_empresa");

                entity.Property(e => e.DtAgendamento)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_agendamento")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.DtAgendamentoFim)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_agendamento_fim");

                entity.Property(e => e.VlFechamento)
                    .HasColumnType("float unsigned")
                    .HasColumnName("vl_fechamento");
            });

            modelBuilder.Entity<CaoDocumentacaoCasosUso>(entity =>
            {
                entity.ToTable("cao_documentacao_casos_uso");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.Nome)
                    .HasMaxLength(70)
                    .HasColumnName("nome")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoDocumentacaoOutro>(entity =>
            {
                entity.ToTable("cao_documentacao_outros");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.Nome)
                    .HasMaxLength(70)
                    .HasColumnName("nome")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoDocumentacaoSistema>(entity =>
            {
                entity.ToTable("cao_documentacao_sistema");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Arquivo)
                    .HasMaxLength(100)
                    .HasColumnName("arquivo")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(60)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DtDoc)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_doc")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Pasta)
                    .HasMaxLength(30)
                    .HasColumnName("pasta")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.SubGrupo).HasColumnName("sub_grupo");
            });

            modelBuilder.Entity<CaoDrAtivComp>(entity =>
            {
                entity.HasKey(e => e.IdAtivComp)
                    .HasName("PRIMARY");

                entity.ToTable("cao_dr_ativ_comp");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoUsuario, "co_usuario");

                entity.Property(e => e.IdAtivComp).HasColumnName("id_ativ_comp");

                entity.Property(e => e.Assunto)
                    .HasColumnType("text")
                    .HasColumnName("assunto");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.TempoGasto)
                    .HasColumnType("time")
                    .HasColumnName("tempo_gasto")
                    .HasDefaultValueSql("'00:00:00'");
            });

            modelBuilder.Entity<CaoEscalaRanking>(entity =>
            {
                entity.HasKey(e => e.Idescala)
                    .HasName("PRIMARY");

                entity.ToTable("cao_escala_ranking");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Idescala).HasColumnName("idescala");

                entity.Property(e => e.Pontuacao).HasColumnName("pontuacao");

                entity.Property(e => e.QtdVisual).HasColumnName("qtd_visual");
            });

            modelBuilder.Entity<CaoEscritorio>(entity =>
            {
                entity.HasKey(e => e.CoEscritorio)
                    .HasName("PRIMARY");

                entity.ToTable("cao_escritorio");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoEscritorio)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("co_escritorio");

                entity.Property(e => e.Local)
                    .HasMaxLength(2)
                    .HasColumnName("local")
                    .HasDefaultValueSql("''")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CaoFatura>(entity =>
            {
                entity.HasKey(e => e.CoFatura)
                    .HasName("PRIMARY");

                entity.ToTable("cao_fatura");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoFatura).HasColumnName("co_fatura");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoOs).HasColumnName("co_os");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.ComissaoCn).HasColumnName("comissao_cn");

                entity.Property(e => e.CorpoNf)
                    .HasColumnType("text")
                    .HasColumnName("corpo_nf");

                entity.Property(e => e.DataEmissao)
                    .HasColumnName("data_emissao")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.NumNf).HasColumnName("num_nf");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.TotalImpInc).HasColumnName("total_imp_inc");

                entity.Property(e => e.Valor).HasColumnName("valor");
            });

            modelBuilder.Entity<CaoFaturaPag>(entity =>
            {
                entity.HasKey(e => e.IdFaturaPag)
                    .HasName("PRIMARY");

                entity.ToTable("cao_fatura_pag");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoFatura, "co_fatura")
                    .IsUnique();

                entity.Property(e => e.IdFaturaPag).HasColumnName("id_fatura_pag");

                entity.Property(e => e.CoFatura).HasColumnName("co_fatura");

                entity.Property(e => e.DtEfetuado)
                    .HasColumnName("dt_efetuado")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.ValorPago).HasColumnName("valor_pago");
            });

            modelBuilder.Entity<CaoFeriado>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cao_feriados");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Ano)
                    .HasColumnName("ano")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Dia)
                    .HasColumnName("dia")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Mes)
                    .HasColumnName("mes")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<CaoFormacaoIdiomaUsuario>(entity =>
            {
                entity.HasKey(e => new { e.CoUsuario, e.CoIdioma })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("cao_formacao_idioma_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoIdioma).HasColumnName("co_idioma");

                entity.Property(e => e.NvEscrita).HasColumnName("nv_escrita");

                entity.Property(e => e.NvFala).HasColumnName("nv_fala");

                entity.Property(e => e.NvLeitura).HasColumnName("nv_leitura");
            });

            modelBuilder.Entity<CaoFormacaoUsuario>(entity =>
            {
                entity.HasKey(e => new { e.CoUsuario, e.TpCurso })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("cao_formacao_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TpCurso)
                    .HasMaxLength(13)
                    .HasColumnName("tp_curso")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DsCurso)
                    .HasMaxLength(50)
                    .HasColumnName("ds_curso");

                entity.Property(e => e.DsInstituicao)
                    .HasMaxLength(50)
                    .HasColumnName("ds_instituicao");

                entity.Property(e => e.DtConclusao).HasColumnName("dt_conclusao");
            });

            modelBuilder.Entity<CaoHelpAutor>(entity =>
            {
                entity.HasKey(e => e.CoAutor)
                    .HasName("PRIMARY");

                entity.ToTable("cao_help_autor");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoAutor, "co_autor");

                entity.Property(e => e.CoAutor).HasColumnName("co_autor");

                entity.Property(e => e.CoFilial).HasColumnName("co_filial");

                entity.Property(e => e.DsEmail)
                    .HasMaxLength(50)
                    .HasColumnName("ds_email");

                entity.Property(e => e.DsFuncao)
                    .HasMaxLength(100)
                    .HasColumnName("ds_funcao")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.NoAutor)
                    .HasMaxLength(80)
                    .HasColumnName("no_autor")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NuDdd1)
                    .HasMaxLength(4)
                    .HasColumnName("nu_ddd1");

                entity.Property(e => e.NuDdd2)
                    .HasMaxLength(4)
                    .HasColumnName("nu_ddd2");

                entity.Property(e => e.NuRamal1)
                    .HasMaxLength(6)
                    .HasColumnName("nu_ramal1");

                entity.Property(e => e.NuRamal2)
                    .HasMaxLength(6)
                    .HasColumnName("nu_ramal2");

                entity.Property(e => e.NuTel1)
                    .HasMaxLength(15)
                    .HasColumnName("nu_tel1");

                entity.Property(e => e.NuTel2)
                    .HasMaxLength(15)
                    .HasColumnName("nu_tel2");
            });

            modelBuilder.Entity<CaoHelpChamado>(entity =>
            {
                entity.HasKey(e => e.CoChamado)
                    .HasName("PRIMARY");

                entity.ToTable("cao_help_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoChamado, "co_chamado");

                entity.Property(e => e.CoChamado).HasColumnName("co_chamado");

                entity.Property(e => e.CoAutor).HasColumnName("co_autor");

                entity.Property(e => e.CoFilial).HasColumnName("co_filial");

                entity.Property(e => e.CoMotivo).HasColumnName("co_motivo");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoStatus).HasColumnName("co_status");

                entity.Property(e => e.CoTela).HasColumnName("co_tela");

                entity.Property(e => e.DsChamado)
                    .HasColumnType("text")
                    .HasColumnName("ds_chamado");

                entity.Property(e => e.DsSolucao)
                    .HasColumnType("text")
                    .HasColumnName("ds_solucao");

                entity.Property(e => e.DtChamado)
                    .HasColumnName("dt_chamado")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.DtSolucao).HasColumnName("dt_solucao");
            });

            modelBuilder.Entity<CaoHelpFilial>(entity =>
            {
                entity.HasKey(e => e.CoFilial)
                    .HasName("PRIMARY");

                entity.ToTable("cao_help_filial");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoFilial, "filialId");

                entity.Property(e => e.CoFilial).HasColumnName("co_filial");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.Estado)
                    .HasMaxLength(2)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("''")
                    .IsFixedLength();

                entity.Property(e => e.NoFilial)
                    .HasMaxLength(70)
                    .HasColumnName("no_filial")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoHelpMotivoChamado>(entity =>
            {
                entity.HasKey(e => e.CoMotivo)
                    .HasName("PRIMARY");

                entity.ToTable("cao_help_motivo_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoMotivo, "co_motivo")
                    .IsUnique();

                entity.Property(e => e.CoMotivo).HasColumnName("co_motivo");

                entity.Property(e => e.DsMotivo)
                    .HasMaxLength(70)
                    .HasColumnName("ds_motivo")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<CaoHelpStatusChamado>(entity =>
            {
                entity.HasKey(e => e.CoStatus)
                    .HasName("PRIMARY");

                entity.ToTable("cao_help_status_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoStatus, "co_status")
                    .IsUnique();

                entity.Property(e => e.CoStatus).HasColumnName("co_status");

                entity.Property(e => e.DsStatus)
                    .HasMaxLength(50)
                    .HasColumnName("ds_status")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<CaoHelpTelaChamado>(entity =>
            {
                entity.HasKey(e => e.CoTela)
                    .HasName("PRIMARY");

                entity.ToTable("cao_help_tela_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoTela).HasColumnName("co_tela");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.DsTela)
                    .HasMaxLength(200)
                    .HasColumnName("ds_tela")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoHistOcorrenciasO>(entity =>
            {
                entity.HasKey(e => e.Idocorrencia)
                    .HasName("PRIMARY");

                entity.ToTable("cao_hist_ocorrencias_os");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoOs, "co_os");

                entity.HasIndex(e => e.CoUsuario, "co_usuario");

                entity.Property(e => e.Idocorrencia).HasColumnName("idocorrencia");

                entity.Property(e => e.CoOs).HasColumnName("co_os");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data");

                entity.Property(e => e.DataFechamento)
                    .HasColumnType("datetime")
                    .HasColumnName("data_fechamento");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.Responsavel)
                    .HasMaxLength(50)
                    .HasColumnName("responsavel")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tipo)
                    .HasColumnType("enum('Interna','Externa')")
                    .HasColumnName("tipo")
                    .HasDefaultValueSql("'Interna'");

                entity.HasOne(d => d.CoOsNavigation)
                    .WithMany(p => p.CaoHistOcorrenciasOs)
                    .HasForeignKey(d => d.CoOs)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("cao_hist_ocorrencias_os_ibfk_2");

                entity.HasOne(d => d.CoUsuarioNavigation)
                    .WithMany(p => p.CaoHistOcorrenciasOs)
                    .HasForeignKey(d => d.CoUsuario)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("cao_hist_ocorrencias_os_ibfk_1");
            });

            modelBuilder.Entity<CaoHorarioAlmoco>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cao_horario_almoco");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.AlmocoSaidaHora)
                    .HasMaxLength(128)
                    .HasColumnName("almoco_saida_hora")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AlmocoVoltaHora)
                    .HasMaxLength(128)
                    .HasColumnName("almoco_volta_hora")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoLogChamado>(entity =>
            {
                entity.ToTable("cao_log_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoChamado).HasColumnName("co_chamado");

                entity.Property(e => e.CoDaily).HasColumnName("co_daily");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(128)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtChamado)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_chamado")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");
            });

            modelBuilder.Entity<CaoMenu>(entity =>
            {
                entity.HasKey(e => e.CoMenu)
                    .HasName("PRIMARY");

                entity.ToTable("cao_menu");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoMenu)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("co_menu");

                entity.Property(e => e.DsImagem)
                    .HasMaxLength(255)
                    .HasColumnName("ds_imagem");

                entity.Property(e => e.DsMenu)
                    .HasMaxLength(255)
                    .HasColumnName("ds_menu")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DsPagina)
                    .HasMaxLength(255)
                    .HasColumnName("ds_pagina")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoMenuContador>(entity =>
            {
                entity.HasKey(e => new { e.CoUsuario, e.CoMenu })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("cao_menu_contador");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoMenu).HasColumnName("co_menu");

                entity.Property(e => e.InProcessado).HasColumnName("in_processado");

                entity.Property(e => e.NuPontos).HasColumnName("nu_pontos");
            });

            modelBuilder.Entity<CaoMovimento>(entity =>
            {
                entity.HasKey(e => e.CoMovimento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_movimento");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoMovimento, "co_movimento");

                entity.Property(e => e.CoMovimento).HasColumnName("co_movimento");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtEntrada)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_entrada")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.DtSaida)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_saida")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.DtSaidaAlmoco)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_saida_almoco")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.DtVoltaAlmoco)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_volta_almoco")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.IsEncerrado).HasColumnName("is_encerrado");
            });

            modelBuilder.Entity<CaoMovimentoJustificativa>(entity =>
            {
                entity.HasKey(e => e.CoMovimentoJustificativa)
                    .HasName("PRIMARY");

                entity.ToTable("cao_movimento_justificativa");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoMovimentoJustificativa, "co_movimento_justificativa");

                entity.Property(e => e.CoMovimentoJustificativa).HasColumnName("co_movimento_justificativa");

                entity.Property(e => e.CoMovimento).HasColumnName("co_movimento");

                entity.Property(e => e.DsJustificativa).HasColumnName("ds_justificativa");

                entity.Property(e => e.NuOs).HasColumnName("nu_os");
            });

            modelBuilder.Entity<CaoMovimentoO>(entity =>
            {
                entity.HasKey(e => e.CoMovimentoOs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_movimento_os");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoMovimentoOs).HasColumnName("co_movimento_os");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoTipoMovimento).HasColumnName("co_tipo_movimento");

                entity.Property(e => e.DsValor).HasColumnName("ds_valor");

                entity.Property(e => e.DtFim)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_fim");

                entity.Property(e => e.DtIni)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_ini");

                entity.Property(e => e.NuOs).HasColumnName("nu_os");

                entity.Property(e => e.NuValor).HasColumnName("nu_valor");

                entity.Property(e => e.UsuarioObs)
                    .HasMaxLength(30)
                    .HasColumnName("usuario_obs");
            });

            modelBuilder.Entity<CaoNoticium>(entity =>
            {
                entity.HasKey(e => e.CoNoticia)
                    .HasName("PRIMARY");

                entity.ToTable("cao_noticia");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoNoticia).HasColumnName("co_noticia");

                entity.Property(e => e.Assunto)
                    .HasMaxLength(255)
                    .HasColumnName("assunto")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(60)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DtNoticia)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_noticia")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Foto)
                    .HasMaxLength(255)
                    .HasColumnName("foto")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoO>(entity =>
            {
                entity.HasKey(e => e.CoOs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoOs).HasColumnName("co_os");

                entity.Property(e => e.CoArquitetura)
                    .HasColumnName("co_arquitetura")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoEmail).HasColumnName("co_email");

                entity.Property(e => e.CoOsProspectRel).HasColumnName("co_os_prospect_rel");

                entity.Property(e => e.CoSistema)
                    .HasColumnName("co_sistema")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoStatus)
                    .HasColumnName("co_status")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DddTelSol)
                    .HasMaxLength(5)
                    .HasColumnName("ddd_tel_sol");

                entity.Property(e => e.DddTelSol2)
                    .HasMaxLength(5)
                    .HasColumnName("ddd_tel_sol2");

                entity.Property(e => e.DiretoriaSol)
                    .HasMaxLength(50)
                    .HasColumnName("diretoria_sol")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DsCaracteristica)
                    .HasMaxLength(200)
                    .HasColumnName("ds_caracteristica")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DsOs)
                    .HasMaxLength(200)
                    .HasColumnName("ds_os")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DsRequisito)
                    .HasMaxLength(200)
                    .HasColumnName("ds_requisito");

                entity.Property(e => e.DtFim).HasColumnName("dt_fim");

                entity.Property(e => e.DtGarantia).HasColumnName("dt_garantia");

                entity.Property(e => e.DtImp).HasColumnName("dt_imp");

                entity.Property(e => e.DtInicio).HasColumnName("dt_inicio");

                entity.Property(e => e.DtSol).HasColumnName("dt_sol");

                entity.Property(e => e.NuOs).HasColumnName("nu_os");

                entity.Property(e => e.NuTelSol)
                    .HasMaxLength(20)
                    .HasColumnName("nu_tel_sol")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.NuTelSol2)
                    .HasMaxLength(20)
                    .HasColumnName("nu_tel_sol2");

                entity.Property(e => e.UsuarioSol)
                    .HasMaxLength(50)
                    .HasColumnName("usuario_sol")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<CaoObsCliente>(entity =>
            {
                entity.HasKey(e => e.CoObs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_obs_cliente");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoObs).HasColumnName("co_obs");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(30)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(250)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtObs)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_obs")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");
            });

            modelBuilder.Entity<CaoObsSistema>(entity =>
            {
                entity.HasKey(e => e.CoObs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_obs_sistema");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoObs).HasColumnName("co_obs");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DtObs)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_obs");
            });

            modelBuilder.Entity<CaoOmbudsman>(entity =>
            {
                entity.ToTable("cao_ombudsman");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoEscritorio, "co_escritorio");

                entity.HasIndex(e => e.Idcategoria, "idcategoria");

                entity.HasIndex(e => e.Idtipo, "idtipo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoEscritorio).HasColumnName("co_escritorio");

                entity.Property(e => e.Comentario)
                    .HasColumnType("text")
                    .HasColumnName("comentario");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Idcategoria).HasColumnName("idcategoria");

                entity.Property(e => e.Idtipo).HasColumnName("idtipo");

                entity.HasOne(d => d.IdcategoriaNavigation)
                    .WithMany(p => p.CaoOmbudsmen)
                    .HasForeignKey(d => d.Idcategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_ombudsman_ibfk_2");

                entity.HasOne(d => d.IdtipoNavigation)
                    .WithMany(p => p.CaoOmbudsmen)
                    .HasForeignKey(d => d.Idtipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_ombudsman_ibfk_1");
            });

            modelBuilder.Entity<CaoOsAnalistum>(entity =>
            {
                entity.HasKey(e => e.CoAnalista)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_analista");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoAnalista).HasColumnName("co_analista");

                entity.Property(e => e.CoOs)
                    .HasColumnName("co_os")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<CaoOsChamado>(entity =>
            {
                entity.HasKey(e => e.CoChamado)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoChamado).HasColumnName("co_chamado");

                entity.Property(e => e.CoAnalista)
                    .HasMaxLength(255)
                    .HasColumnName("co_analista")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoItem).HasColumnName("co_item");

                entity.Property(e => e.CoOs).HasColumnName("co_os");

                entity.Property(e => e.CoPrioridade).HasColumnName("co_prioridade");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoTipo).HasColumnName("co_tipo");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DsChamado)
                    .HasMaxLength(255)
                    .HasColumnName("ds_chamado")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DsSolucao)
                    .HasColumnType("text")
                    .HasColumnName("ds_solucao");

                entity.Property(e => e.DtChamado)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_chamado")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Tempo)
                    .HasMaxLength(30)
                    .HasColumnName("tempo")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoOsDailyReport>(entity =>
            {
                entity.HasKey(e => e.CoDaily)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_daily_report");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoDaily).HasColumnName("co_daily");

                entity.Property(e => e.CoAtividade).HasColumnName("co_atividade");

                entity.Property(e => e.CoChamado).HasColumnName("co_chamado");

                entity.Property(e => e.CoFase)
                    .HasColumnName("co_fase")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoOs)
                    .HasColumnName("co_os")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoStatusAtual).HasColumnName("co_status_atual");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("co_usuario");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data");

                entity.Property(e => e.DsAssunto)
                    .HasColumnType("text")
                    .HasColumnName("ds_assunto")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.TempoGasto)
                    .HasColumnType("time")
                    .HasColumnName("tempo_gasto");
            });

            modelBuilder.Entity<CaoOsEmail>(entity =>
            {
                entity.HasKey(e => e.CoEmail)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_email");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoEmail).HasColumnName("co_email");

                entity.Property(e => e.Ativo).HasColumnName("ativo");

                entity.Property(e => e.Cargo)
                    .HasMaxLength(80)
                    .HasColumnName("cargo");

                entity.Property(e => e.CoCliente).HasColumnName("co_cliente");

                entity.Property(e => e.CoOs)
                    .HasColumnName("co_os")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ddd)
                    .HasMaxLength(20)
                    .HasColumnName("ddd");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .HasColumnName("email")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Nome)
                    .HasMaxLength(255)
                    .HasColumnName("nome")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Senha)
                    .HasMaxLength(20)
                    .HasColumnName("senha")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Tel)
                    .HasMaxLength(40)
                    .HasColumnName("tel");
            });

            modelBuilder.Entity<CaoOsFase>(entity =>
            {
                entity.HasKey(e => e.CoFase)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_fase");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoFase).HasColumnName("co_fase");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DescricaoIngl)
                    .HasMaxLength(200)
                    .HasColumnName("descricao_ingl")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Ordem).HasColumnName("ordem");
            });

            modelBuilder.Entity<CaoOsItemMenu>(entity =>
            {
                entity.HasKey(e => e.CoItem)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_item_menu");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoItem).HasColumnName("co_item");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.DsItem)
                    .HasMaxLength(255)
                    .HasColumnName("ds_item")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoOsOb>(entity =>
            {
                entity.HasKey(e => e.CoObs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_obs");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoObs).HasColumnName("co_obs");

                entity.Property(e => e.CoOs)
                    .HasColumnName("co_os")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(80)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DtObs)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_obs")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");
            });

            modelBuilder.Entity<CaoOsObsChamado>(entity =>
            {
                entity.HasKey(e => e.CoObs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_obs_chamado");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoObs).HasColumnName("co_obs");

                entity.Property(e => e.ArquivoObs)
                    .HasMaxLength(255)
                    .HasColumnName("arquivo_obs");

                entity.Property(e => e.CoChamado)
                    .HasColumnName("co_chamado")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(80)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DtObs)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_obs")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");
            });

            modelBuilder.Entity<CaoOsPrazo>(entity =>
            {
                entity.HasKey(e => e.CoPrazo)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_prazo");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoPrazo).HasColumnName("co_prazo");

                entity.Property(e => e.CoFase)
                    .HasColumnName("co_fase")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoOs)
                    .HasColumnName("co_os")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalAnalista)
                    .HasColumnName("total_analista")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalHora)
                    .HasColumnName("total_hora")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<CaoOsStatus>(entity =>
            {
                entity.HasKey(e => e.CoStatusAtual)
                    .HasName("PRIMARY");

                entity.ToTable("cao_os_status");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoStatusAtual).HasColumnName("co_status_atual");

                entity.Property(e => e.DsStatus)
                    .HasMaxLength(50)
                    .HasColumnName("ds_status")
                    .HasDefaultValueSql("''")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CaoPagamento>(entity =>
            {
                entity.HasKey(e => e.CoPagamento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_pagamento");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoPagamento, "nu_os");

                entity.Property(e => e.CoPagamento).HasColumnName("co_pagamento");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtPagamento)
                    .HasColumnName("dt_pagamento")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.DtReferenciaPagamento)
                    .HasColumnName("dt_referencia_pagamento")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.TpPagamento)
                    .HasMaxLength(2)
                    .HasColumnName("tp_pagamento")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength();

                entity.Property(e => e.VlPagamento).HasColumnName("vl_pagamento");
            });

            modelBuilder.Entity<CaoParticipacaoFuncionario>(entity =>
            {
                entity.HasKey(e => e.CoPartFuncionario)
                    .HasName("PRIMARY");

                entity.ToTable("cao_participacao_funcionario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoPartFuncionario).HasColumnName("co_part_funcionario");

                entity.Property(e => e.CoEscritorio).HasColumnName("co_escritorio");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtReferencia)
                    .HasColumnName("dt_referencia")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.PcParticipacao)
                    .HasColumnType("float unsigned")
                    .HasColumnName("pc_participacao");
            });

            modelBuilder.Entity<CaoPermissao>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cao_permissao");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoUsuario, "co_usuario");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PermissaoHoraExtra)
                    .HasColumnType("enum('0','1')")
                    .HasColumnName("permissao_hora_extra")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PermissaoIntervalo)
                    .HasColumnType("enum('0','1')")
                    .HasColumnName("permissao_intervalo")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.CoUsuarioNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_permissao_ibfk_1");
            });

            modelBuilder.Entity<CaoPermissaoHoraExtra>(entity =>
            {
                entity.HasKey(e => e.IdPermissao)
                    .HasName("PRIMARY");

                entity.ToTable("cao_permissao_hora_extra");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.IdPermissao).HasColumnName("id_permissao");

                entity.Property(e => e.CoMovimento).HasColumnName("co_movimento");
            });

            modelBuilder.Entity<CaoPontosConhecimento>(entity =>
            {
                entity.HasKey(e => e.Idpontos)
                    .HasName("PRIMARY");

                entity.ToTable("cao_pontos_conhecimento");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoCoordenador, "co_coordenador");

                entity.HasIndex(e => e.Idconhecimento, "idconhecimento");

                entity.HasIndex(e => new { e.Pontuacao, e.CoCoordenador, e.Idconhecimento }, "pontuacao");

                entity.Property(e => e.Idpontos).HasColumnName("idpontos");

                entity.Property(e => e.CoCoordenador)
                    .HasMaxLength(50)
                    .HasColumnName("co_coordenador")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Idconhecimento).HasColumnName("idconhecimento");

                entity.Property(e => e.Pontuacao).HasColumnName("pontuacao");

                entity.HasOne(d => d.CoCoordenadorNavigation)
                    .WithMany(p => p.CaoPontosConhecimentos)
                    .HasForeignKey(d => d.CoCoordenador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_pontos_conhecimento_ibfk_2");

                entity.HasOne(d => d.IdconhecimentoNavigation)
                    .WithMany(p => p.CaoPontosConhecimentos)
                    .HasForeignKey(d => d.Idconhecimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_pontos_conhecimento_ibfk_3");

                entity.HasOne(d => d.PontuacaoNavigation)
                    .WithMany(p => p.CaoPontosConhecimentos)
                    .HasForeignKey(d => d.Pontuacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_pontos_conhecimento_ibfk_1");
            });

            modelBuilder.Entity<CaoRamo>(entity =>
            {
                entity.HasKey(e => e.CoRamo)
                    .HasName("PRIMARY");

                entity.ToTable("cao_ramo");

                entity.HasComment("Ramo de atividade de Clientes")
                    .HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoRamo).HasColumnName("co_ramo");

                entity.Property(e => e.DsRamo)
                    .HasMaxLength(60)
                    .HasColumnName("ds_ramo")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoRelEmailO>(entity =>
            {
                entity.ToTable("cao_rel_email_os");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoEmail).HasColumnName("co_email");

                entity.Property(e => e.CoOs).HasColumnName("co_os");
            });

            modelBuilder.Entity<CaoSalario>(entity =>
            {
                entity.HasKey(e => new { e.CoUsuario, e.DtAlteracao })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("cao_salario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtAlteracao)
                    .HasColumnName("dt_alteracao")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.BrutSalario).HasColumnName("brut_salario");

                entity.Property(e => e.LiqSalario).HasColumnName("liq_salario");
            });

            modelBuilder.Entity<CaoSalarioPag>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cao_salario_pag");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.IdPagamento, "id_pagamento");

                entity.HasIndex(e => new { e.CoUsuario, e.DtEfetuado }, "unico")
                    .IsUnique();

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DtEfetuado)
                    .HasColumnName("dt_efetuado")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.IdPagamento)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_pagamento");

                entity.Property(e => e.Observacao)
                    .HasColumnType("text")
                    .HasColumnName("observacao");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('Autorizado','Pago','Pendente')")
                    .HasColumnName("status")
                    .HasDefaultValueSql("'Pendente'");

                entity.HasOne(d => d.CoUsuarioNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CoUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cao_salario_pag_ibfk_1");
            });

            modelBuilder.Entity<CaoServico>(entity =>
            {
                entity.HasKey(e => e.CoServico)
                    .HasName("PRIMARY");

                entity.ToTable("cao_servico");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoServico).HasColumnName("co_servico");

                entity.Property(e => e.DsServico)
                    .HasMaxLength(50)
                    .HasColumnName("ds_servico")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoSistema>(entity =>
            {
                entity.HasKey(e => e.CoSistema)
                    .HasName("PRIMARY");

                entity.ToTable("cao_sistema");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoSistema, "co_sistema");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoArquitetura)
                    .HasColumnName("co_arquitetura")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoCliente)
                    .HasColumnName("co_cliente")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoEmail).HasColumnName("co_email");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DddTelefoneSolic)
                    .HasMaxLength(5)
                    .HasColumnName("ddd_telefone_solic");

                entity.Property(e => e.DsCaracteristica)
                    .HasColumnType("text")
                    .HasColumnName("ds_caracteristica");

                entity.Property(e => e.DsRequisito)
                    .HasColumnType("text")
                    .HasColumnName("ds_requisito");

                entity.Property(e => e.DsSistemaResumo)
                    .HasColumnType("text")
                    .HasColumnName("ds_sistema_resumo");

                entity.Property(e => e.DtEntrega).HasColumnName("dt_entrega");

                entity.Property(e => e.DtSolicitacao).HasColumnName("dt_solicitacao");

                entity.Property(e => e.NoDiretoriaSolic)
                    .HasMaxLength(100)
                    .HasColumnName("no_diretoria_solic");

                entity.Property(e => e.NoSistema)
                    .HasMaxLength(200)
                    .HasColumnName("no_sistema");

                entity.Property(e => e.NoUsuarioSolic)
                    .HasMaxLength(100)
                    .HasColumnName("no_usuario_solic");

                entity.Property(e => e.NuTelefoneSolic)
                    .HasMaxLength(20)
                    .HasColumnName("nu_telefone_solic");
            });

            modelBuilder.Entity<CaoSistemaOb>(entity =>
            {
                entity.HasKey(e => e.CoObs)
                    .HasName("PRIMARY");

                entity.ToTable("cao_sistema_obs");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoObs).HasColumnName("co_obs");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.DtObs)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_obs");
            });

            modelBuilder.Entity<CaoStatusAgendamento>(entity =>
            {
                entity.HasKey(e => e.CoStatusAgendamento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_status_agendamento");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoStatusAgendamento).HasColumnName("co_status_agendamento");

                entity.Property(e => e.DsStatusAgendamento)
                    .HasMaxLength(20)
                    .HasColumnName("ds_status_agendamento")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoStatusCliente>(entity =>
            {
                entity.HasKey(e => e.CoStatus)
                    .HasName("PRIMARY");

                entity.ToTable("cao_status_cliente");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoStatus).HasColumnName("co_status");

                entity.Property(e => e.DsStatus)
                    .HasMaxLength(100)
                    .HasColumnName("ds_status")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoStatusClienteComplemento>(entity =>
            {
                entity.HasKey(e => e.CoComplementoStatus)
                    .HasName("PRIMARY");

                entity.ToTable("cao_status_cliente_complemento");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoComplementoStatus).HasColumnName("co_complemento_status");

                entity.Property(e => e.CoStatus).HasColumnName("co_status");

                entity.Property(e => e.DsStatus)
                    .HasMaxLength(50)
                    .HasColumnName("ds_status")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoStatusO>(entity =>
            {
                entity.HasKey(e => e.CoStatusAtual)
                    .HasName("PRIMARY");

                entity.ToTable("cao_status_os");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoStatusAtual).HasColumnName("co_status_atual");

                entity.Property(e => e.DsStatus)
                    .HasMaxLength(50)
                    .HasColumnName("ds_status")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoTempImport>(entity =>
            {
                entity.HasKey(e => e.Codigo)
                    .HasName("PRIMARY");

                entity.ToTable("cao_temp_import");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Codigo).HasColumnName("codigo");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(60)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoTipoConhecimentoUsuario>(entity =>
            {
                entity.HasKey(e => e.CoConhecimento)
                    .HasName("PRIMARY");

                entity.ToTable("cao_tipo_conhecimento_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoConhecimento, "co_conhecimento");

                entity.Property(e => e.CoConhecimento).HasColumnName("co_conhecimento");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.DsConhecimento)
                    .HasMaxLength(20)
                    .HasColumnName("ds_conhecimento");
            });

            modelBuilder.Entity<CaoTipoCusto>(entity =>
            {
                entity.HasKey(e => e.CoTipoCusto)
                    .HasName("PRIMARY");

                entity.ToTable("cao_tipo_custo");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoTipoCusto)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("co_tipo_custo");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(30)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoTipoIdiomaUsuario>(entity =>
            {
                entity.HasKey(e => e.CoIdioma)
                    .HasName("PRIMARY");

                entity.ToTable("cao_tipo_idioma_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoIdioma).HasColumnName("co_idioma");

                entity.Property(e => e.DsIdioma)
                    .HasMaxLength(13)
                    .HasColumnName("ds_idioma");
            });

            modelBuilder.Entity<CaoTipoOmbudsman>(entity =>
            {
                entity.HasKey(e => e.Idtipo)
                    .HasName("PRIMARY");

                entity.ToTable("cao_tipo_ombudsman");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Idtipo).HasColumnName("idtipo");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(100)
                    .HasColumnName("descricao")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<CaoTipoSistemaUsuario>(entity =>
            {
                entity.HasKey(e => e.CoSistema)
                    .HasName("PRIMARY");

                entity.ToTable("cao_tipo_sistema_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoSistema, "co_sistema");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.DsSistema)
                    .HasMaxLength(40)
                    .HasColumnName("ds_sistema");
            });

            modelBuilder.Entity<CaoUf>(entity =>
            {
                entity.HasKey(e => e.CoUf)
                    .HasName("PRIMARY");

                entity.ToTable("cao_uf");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoUf).HasColumnName("co_uf");

                entity.Property(e => e.DsUf)
                    .HasMaxLength(5)
                    .HasColumnName("ds_uf")
                    .HasDefaultValueSql("''")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CaoUsuario>(entity =>
            {
                entity.HasKey(e => e.CoUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("cao_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.CoUsuario, "co_usuario")
                    .IsUnique();

                entity.HasIndex(e => new { e.CoUsuario, e.NoUsuario, e.DtAlteracao }, "co_usuario_2");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoUsuarioAutorizacao)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario_autorizacao")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DsBairro)
                    .HasMaxLength(30)
                    .HasColumnName("ds_bairro")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DsCompEnd)
                    .HasMaxLength(50)
                    .HasColumnName("ds_comp_end")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DsEndereco)
                    .HasMaxLength(150)
                    .HasColumnName("ds_endereco")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DsSenha)
                    .HasMaxLength(14)
                    .HasColumnName("ds_senha")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DtAdmissaoEmpresa)
                    .HasColumnName("dt_admissao_empresa")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.DtAlteracao)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_alteracao")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.DtDesligamento).HasColumnName("dt_desligamento");

                entity.Property(e => e.DtExpedicao).HasColumnName("dt_expedicao");

                entity.Property(e => e.DtExpiracao)
                    .HasColumnName("dt_expiracao")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.DtInclusao)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_inclusao")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.DtNascimento)
                    .HasColumnName("dt_nascimento")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.Icq).HasColumnName("icq");

                entity.Property(e => e.InstantMessenger)
                    .HasMaxLength(80)
                    .HasColumnName("instant_messenger")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Msn)
                    .HasMaxLength(50)
                    .HasColumnName("msn")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NoCidade)
                    .HasMaxLength(50)
                    .HasColumnName("no_cidade")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NoEmail)
                    .HasMaxLength(100)
                    .HasColumnName("no_email")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NoEmailPessoal)
                    .HasMaxLength(100)
                    .HasColumnName("no_email_pessoal")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NoOrgaoEmissor)
                    .HasMaxLength(10)
                    .HasColumnName("no_orgao_emissor")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NoUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("no_usuario")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NuCep)
                    .HasMaxLength(10)
                    .HasColumnName("nu_cep")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NuCpf)
                    .HasMaxLength(14)
                    .HasColumnName("nu_cpf")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NuMatricula).HasColumnName("nu_matricula");

                entity.Property(e => e.NuRg)
                    .HasMaxLength(20)
                    .HasColumnName("nu_rg")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.NuTelefone)
                    .HasMaxLength(64)
                    .HasColumnName("nu_telefone")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UfCidade)
                    .HasMaxLength(2)
                    .HasColumnName("uf_cidade")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UfOrgaoEmissor)
                    .HasMaxLength(2)
                    .HasColumnName("uf_orgao_emissor")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(255)
                    .HasColumnName("url_foto")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.Yms)
                    .HasMaxLength(50)
                    .HasColumnName("yms")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");
            });

            modelBuilder.Entity<CaoUsuarioDtDisp>(entity =>
            {
                entity.HasKey(e => e.CoDtDisp)
                    .HasName("PRIMARY");

                entity.ToTable("cao_usuario_dt_disp");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.CoDtDisp).HasColumnName("co_dt_disp");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario");

                entity.Property(e => e.DtAlt)
                    .HasColumnName("dt_alt")
                    .HasDefaultValueSql("'0000-00-00'");

                entity.Property(e => e.DtDisp)
                    .HasColumnName("dt_disp")
                    .HasDefaultValueSql("'0000-00-00'");
            });

            modelBuilder.Entity<CaoValorDescanso>(entity =>
            {
                entity.ToTable("cao_valor_descanso");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(40)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.MesReferencia)
                    .HasMaxLength(50)
                    .HasColumnName("mes_referencia")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Segundos)
                    .HasMaxLength(50)
                    .HasColumnName("segundos")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<PermissaoSistema>(entity =>
            {
                entity.HasKey(e => new { e.CoUsuario, e.CoTipoUsuario, e.CoSistema })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("permissao_sistema");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => new { e.CoUsuario, e.CoTipoUsuario, e.CoSistema, e.DtAtualizacao }, "co_usuario");

                entity.Property(e => e.CoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CoTipoUsuario).HasColumnName("co_tipo_usuario");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.CoUsuarioAtualizacao)
                    .HasMaxLength(20)
                    .HasColumnName("co_usuario_atualizacao")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.DtAtualizacao)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_atualizacao")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.InAtivo)
                    .HasMaxLength(1)
                    .HasColumnName("in_ativo")
                    .HasDefaultValueSql("'S'")
                    .IsFixedLength();
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => new { e.CoTipoUsuario, e.CoSistema })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tipo_usuario");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => new { e.CoTipoUsuario, e.CoSistema }, "co_tipo_usuario");

                entity.Property(e => e.CoTipoUsuario).HasColumnName("co_tipo_usuario");

                entity.Property(e => e.CoSistema).HasColumnName("co_sistema");

                entity.Property(e => e.DsTipoUsuario)
                    .HasMaxLength(32)
                    .HasColumnName("ds_tipo_usuario")
                    .HasDefaultValueSql("''")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");
            });
            
            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        #endregion

        #region Data Sets
        public virtual DbSet<CaoAcompanhamentoSistema> CaoAcompanhamentoSistemas { get; set; } = null!;
        public virtual DbSet<CaoAgendamento> CaoAgendamentos { get; set; } = null!;
        public virtual DbSet<CaoArquiteturaO> CaoArquiteturaOs { get; set; } = null!;
        public virtual DbSet<CaoAtividade> CaoAtividades { get; set; } = null!;
        public virtual DbSet<CaoAtividadeConsultor> CaoAtividadeConsultors { get; set; } = null!;
        public virtual DbSet<CaoAtividadeReport> CaoAtividadeReports { get; set; } = null!;
        public virtual DbSet<CaoAtividadeTeste> CaoAtividadeTestes { get; set; } = null!;
        public virtual DbSet<CaoAviso> CaoAvisos { get; set; } = null!;
        public virtual DbSet<CaoBancoDeHora> CaoBancoDeHoras { get; set; } = null!;
        public virtual DbSet<CaoBancoHora> CaoBancoHoras { get; set; } = null!;
        public virtual DbSet<CaoBoleto> CaoBoletos { get; set; } = null!;
        public virtual DbSet<CaoBonu> CaoBonus { get; set; } = null!;
        public virtual DbSet<CaoBonusStatus> CaoBonusStatuses { get; set; } = null!;
        public virtual DbSet<CaoCategoriasOmbudsman> CaoCategoriasOmbudsmen { get; set; } = null!;
        public virtual DbSet<CaoCidade> CaoCidades { get; set; } = null!;
        public virtual DbSet<CaoCliente> CaoClientes { get; set; } = null!;
        public virtual DbSet<CaoClienteContato> CaoClienteContatos { get; set; } = null!;
        public virtual DbSet<CaoComissao> CaoComissaos { get; set; } = null!;
        public virtual DbSet<CaoComplemento> CaoComplementos { get; set; } = null!;
        public virtual DbSet<CaoConhecimento> CaoConhecimentos { get; set; } = null!;
        public virtual DbSet<CaoConhecimentoUsuario> CaoConhecimentoUsuarios { get; set; } = null!;
        public virtual DbSet<CaoConhecimentosFonte> CaoConhecimentosFontes { get; set; } = null!;
        public virtual DbSet<CaoCusto> CaoCustos { get; set; } = null!;
        public virtual DbSet<CaoDiaryReport> CaoDiaryReports { get; set; } = null!;
        public virtual DbSet<CaoDiaryReportConsultor> CaoDiaryReportConsultors { get; set; } = null!;
        public virtual DbSet<CaoDocumentacaoCasosUso> CaoDocumentacaoCasosUsos { get; set; } = null!;
        public virtual DbSet<CaoDocumentacaoOutro> CaoDocumentacaoOutros { get; set; } = null!;
        public virtual DbSet<CaoDocumentacaoSistema> CaoDocumentacaoSistemas { get; set; } = null!;
        public virtual DbSet<CaoDrAtivComp> CaoDrAtivComps { get; set; } = null!;
        public virtual DbSet<CaoEscalaRanking> CaoEscalaRankings { get; set; } = null!;
        public virtual DbSet<CaoEscritorio> CaoEscritorios { get; set; } = null!;
        public virtual DbSet<CaoFatura> CaoFaturas { get; set; } = null!;
        public virtual DbSet<CaoFaturaPag> CaoFaturaPags { get; set; } = null!;
        public virtual DbSet<CaoFeriado> CaoFeriados { get; set; } = null!;
        public virtual DbSet<CaoFormacaoIdiomaUsuario> CaoFormacaoIdiomaUsuarios { get; set; } = null!;
        public virtual DbSet<CaoFormacaoUsuario> CaoFormacaoUsuarios { get; set; } = null!;
        public virtual DbSet<CaoHelpAutor> CaoHelpAutors { get; set; } = null!;
        public virtual DbSet<CaoHelpChamado> CaoHelpChamados { get; set; } = null!;
        public virtual DbSet<CaoHelpFilial> CaoHelpFilials { get; set; } = null!;
        public virtual DbSet<CaoHelpMotivoChamado> CaoHelpMotivoChamados { get; set; } = null!;
        public virtual DbSet<CaoHelpStatusChamado> CaoHelpStatusChamados { get; set; } = null!;
        public virtual DbSet<CaoHelpTelaChamado> CaoHelpTelaChamados { get; set; } = null!;
        public virtual DbSet<CaoHistOcorrenciasO> CaoHistOcorrenciasOs { get; set; } = null!;
        public virtual DbSet<CaoHorarioAlmoco> CaoHorarioAlmocos { get; set; } = null!;
        public virtual DbSet<CaoLogChamado> CaoLogChamados { get; set; } = null!;
        public virtual DbSet<CaoMenu> CaoMenus { get; set; } = null!;
        public virtual DbSet<CaoMenuContador> CaoMenuContadors { get; set; } = null!;
        public virtual DbSet<CaoMovimento> CaoMovimentos { get; set; } = null!;
        public virtual DbSet<CaoMovimentoJustificativa> CaoMovimentoJustificativas { get; set; } = null!;
        public virtual DbSet<CaoMovimentoO> CaoMovimentoOs { get; set; } = null!;
        public virtual DbSet<CaoNoticium> CaoNoticia { get; set; } = null!;
        public virtual DbSet<CaoO> CaoOs { get; set; } = null!;
        public virtual DbSet<CaoObsCliente> CaoObsClientes { get; set; } = null!;
        public virtual DbSet<CaoObsSistema> CaoObsSistemas { get; set; } = null!;
        public virtual DbSet<CaoOmbudsman> CaoOmbudsmen { get; set; } = null!;
        public virtual DbSet<CaoOsAnalistum> CaoOsAnalista { get; set; } = null!;
        public virtual DbSet<CaoOsChamado> CaoOsChamados { get; set; } = null!;
        public virtual DbSet<CaoOsDailyReport> CaoOsDailyReports { get; set; } = null!;
        public virtual DbSet<CaoOsEmail> CaoOsEmails { get; set; } = null!;
        public virtual DbSet<CaoOsFase> CaoOsFases { get; set; } = null!;
        public virtual DbSet<CaoOsItemMenu> CaoOsItemMenus { get; set; } = null!;
        public virtual DbSet<CaoOsOb> CaoOsObs { get; set; } = null!;
        public virtual DbSet<CaoOsObsChamado> CaoOsObsChamados { get; set; } = null!;
        public virtual DbSet<CaoOsPrazo> CaoOsPrazos { get; set; } = null!;
        public virtual DbSet<CaoOsStatus> CaoOsStatuses { get; set; } = null!;
        public virtual DbSet<CaoPagamento> CaoPagamentos { get; set; } = null!;
        public virtual DbSet<CaoParticipacaoFuncionario> CaoParticipacaoFuncionarios { get; set; } = null!;
        public virtual DbSet<CaoPermissao> CaoPermissaos { get; set; } = null!;
        public virtual DbSet<CaoPermissaoHoraExtra> CaoPermissaoHoraExtras { get; set; } = null!;
        public virtual DbSet<CaoPontosConhecimento> CaoPontosConhecimentos { get; set; } = null!;
        public virtual DbSet<CaoRamo> CaoRamos { get; set; } = null!;
        public virtual DbSet<CaoRelEmailO> CaoRelEmailOs { get; set; } = null!;
        public virtual DbSet<CaoSalario> CaoSalarios { get; set; } = null!;
        public virtual DbSet<CaoSalarioPag> CaoSalarioPags { get; set; } = null!;
        public virtual DbSet<CaoServico> CaoServicos { get; set; } = null!;
        public virtual DbSet<CaoSistema> CaoSistemas { get; set; } = null!;
        public virtual DbSet<CaoSistemaOb> CaoSistemaObs { get; set; } = null!;
        public virtual DbSet<CaoStatusAgendamento> CaoStatusAgendamentos { get; set; } = null!;
        public virtual DbSet<CaoStatusCliente> CaoStatusClientes { get; set; } = null!;
        public virtual DbSet<CaoStatusClienteComplemento> CaoStatusClienteComplementos { get; set; } = null!;
        public virtual DbSet<CaoStatusO> CaoStatusOs { get; set; } = null!;
        public virtual DbSet<CaoTempImport> CaoTempImports { get; set; } = null!;
        public virtual DbSet<CaoTipoConhecimentoUsuario> CaoTipoConhecimentoUsuarios { get; set; } = null!;
        public virtual DbSet<CaoTipoCusto> CaoTipoCustos { get; set; } = null!;
        public virtual DbSet<CaoTipoIdiomaUsuario> CaoTipoIdiomaUsuarios { get; set; } = null!;
        public virtual DbSet<CaoTipoOmbudsman> CaoTipoOmbudsmen { get; set; } = null!;
        public virtual DbSet<CaoTipoSistemaUsuario> CaoTipoSistemaUsuarios { get; set; } = null!;
        public virtual DbSet<CaoUf> CaoUfs { get; set; } = null!;
        public virtual DbSet<CaoUsuario> CaoUsuarios { get; set; } = null!;
        public virtual DbSet<CaoUsuarioDtDisp> CaoUsuarioDtDisps { get; set; } = null!;
        public virtual DbSet<CaoValorDescanso> CaoValorDescansos { get; set; } = null!;
        public virtual DbSet<PermissaoSistema> PermissaoSistemas { get; set; } = null!;
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; } = null!; 
        #endregion
    }
}
