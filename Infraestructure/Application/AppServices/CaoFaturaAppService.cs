using Application.Dtos;
using Application.IAppServices;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.IRepositories;
using Domain.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Specifications;
using Application.IValidator;
using Application.Exceptions;
using System.Dynamic;
using Domain.Interfaces;
using Domain.Extensions;

namespace Infraestructure.Application.AppServices
{
    public partial class CaoFaturaAppService : ICaoFaturaAppService
    {
        private readonly ICaoFaturaRepository _CaoFaturaRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICaoUsuarioAppService _caoUsuarioAppService;

        public CaoFaturaAppService(
            ICaoFaturaRepository CaoFaturaRepository,
            IMapper mapper, IEntityValidator entityValidator, IDateTimeService dateTimeService,
            ICaoUsuarioAppService caoUsuarioAppService)
        {
            _CaoFaturaRepository = CaoFaturaRepository ?? throw new ArgumentNullException(nameof(CaoFaturaRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
            _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            _caoUsuarioAppService = caoUsuarioAppService ?? throw new ArgumentNullException(nameof(caoUsuarioAppService));
        }

        public async Task<bool> AddAsync(CaoFaturaDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoFaturaRepository.AddAsync(_mapper.Map<CaoFatura>(item));
                commited = await _CaoFaturaRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }


        public async Task<List<CaoFaturaDto>> FindAllBySpecificationPatternAsync(Specification<CaoFaturaDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<CaoFaturaDto>>(
                await _CaoFaturaRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<CaoFatura, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<CaoFaturaDto>? specification = null)
        {
            var count = await _CaoFaturaRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<CaoFatura>());
            return count;
        }

        public async Task<CaoFaturaDto> FindOneBySpecificationPatternAsync(Specification<CaoFaturaDto>? specification = null, List<string>? includes = null)
        {
            var item = await _CaoFaturaRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<CaoFatura>(), includes);
            return _mapper.Map<CaoFaturaDto>(item);
        }

        public async Task<List<CaoFaturaDto>> FindPageBySpecificationPatternAsync(Specification<CaoFaturaDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<CaoFaturaDto>>(
                await _CaoFaturaRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<CaoFatura>(), includes, order, pageSize, pageGo));
        }


        //public CaoFaturaDto Get(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoFaturaDto>(_CaoFaturaRepository.Get(id, includes));
        //}

        public async Task<List<CaoFaturaDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _CaoFaturaRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<CaoFaturaDto>>(items.ToList());
            return dtoItems;
        }


        //public async Task<CaoFaturaDto> GetAsync(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoFaturaDto>(await _CaoFaturaRepository.GetAsync(id, includes));
        //}

        //public async Task<bool> RemoveAsync(string id)
        //{
        //    var item = await _CaoFaturaRepository.GetAsync(id);
        //    await _CaoFaturaRepository.DeleteAsync(item);
        //    var commited = await _CaoFaturaRepository.UnitOfWork.CommitAsync();

        //    return commited > 0;
        //}

        //public async Task<bool> UpdateAsync(CaoFaturaDto item)
        //{
        //    int commited;
        //    if (_entityValidator.IsValid(item))
        //    {
        //        await _CaoFaturaRepository.UpdateAsync(_mapper.Map<CaoFatura>(item));
        //        commited = await _CaoFaturaRepository.UnitOfWork.CommitAsync();
        //    }
        //    else
        //        throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
        //    return commited > 0;
        //}

        public async Task<List<CaoFaturaDto>> GetFacturasAsync(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        {
            if (startDate == null)
                startDate = DateTime.MinValue;
            if (endDate == null)
                endDate = DateTime.MaxValue;
            if (coUsuarios == null)
                coUsuarios = new List<string>();

            Expression<Func<CaoFatura, bool>> exp =
                f => coUsuarios.Contains(f.CaoOrdenServicio.CoUsuario)
                && f.DataEmissao >= startDate && f.DataEmissao <= endDate;

            var includes = new List<string>()
            {
                $"{nameof(CaoFatura.CaoOrdenServicio)}",
                $"{nameof(CaoFatura.CaoOrdenServicio)}.{nameof(CaoO.CaoUsuario)}",
                $"{nameof(CaoFatura.CaoOrdenServicio)}.{nameof(CaoO.CaoUsuario)}.{nameof(CaoUsuario.CaoSalario)}"
            };

            var facturas = await _CaoFaturaRepository.FindAllByExpressionAsync(exp, includes);

            return _mapper.Map<List<CaoFaturaDto>>(facturas.ToList());
        }
        public AporteRecetaLiquidaDto GetPizza(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        {
            ArgumentNullException.ThrowIfNull(startDate, nameof(startDate));
            ArgumentNullException.ThrowIfNull(endDate, nameof(endDate));
            ArgumentNullException.ThrowIfNull(coUsuarios, nameof(coUsuarios));

            var listOfUserCodes = coUsuarios.GetAsCsvSingleQuote();

            var query = "select u.co_usuario, u.no_usuario, f.data_emissao, sum(f.valor) as valor, sum(f.valor - (f.valor * f.total_imp_inc/100)) as receita_liquida," +
                "CONCAT(year(f.data_emissao), '-', month(f.data_emissao)) as yearmonth " +
                "from cao_fatura as f " +
                "join cao_os as o on f.co_os = o.co_os join cao_usuario as u on o.co_usuario = u.co_usuario " +
                "where f.data_emissao between {0} and {1} and locate (u.co_usuario, {2}) > 0 " +
                "group by u.co_usuario, yearmonth " +
                "order by u.no_usuario, f.data_emissao";

            var queryResult = _CaoFaturaRepository.UnitOfWork
                .ExecuteQuery<UsuarioRecetaLiquida>(query, new object[]
                {
                    _dateTimeService.ParsedAsMySql( startDate.Value),
                    _dateTimeService.ParsedAsMySql( endDate.Value),
                    listOfUserCodes
                }).ToList();
            var groupedByUsuario = queryResult.GroupBy(u => u.CoUsuario).Select(g => new UsuarioRecetaLiquida
            {
                CoUsuario = g.Key,
                NoUsuario = queryResult.FirstOrDefault(us => us.CoUsuario == g.Key)?.NoUsuario ?? g.Key,
                DataEmissao = queryResult.FirstOrDefault(us => us.CoUsuario == g.Key)?.DataEmissao ?? DateTime.MinValue,
                Yearmonth = queryResult.FirstOrDefault(us => us.CoUsuario == g.Key)?.Yearmonth,
                Valor = queryResult.Where(us => us.CoUsuario == g.Key).Sum(uv => uv.Valor),
                ReceitaLiquida = queryResult.Where(us => us.CoUsuario == g.Key).Sum(uv => uv.ReceitaLiquida),
            }).ToList();

            var total = groupedByUsuario.Sum(u => u.ReceitaLiquida);
            var lista = new List<ValorAporteDto>();
            foreach (var usuario in groupedByUsuario)
            {
                var porCiento = total > 0 ? usuario.ReceitaLiquida / total * 100 : 0.0;
                lista.Add(new ValorAporteDto
                {
                    Code = usuario.CoUsuario,
                    Name = usuario.NoUsuario,
                    RecetaLiquida = usuario.ReceitaLiquida,
                    Porciento = porCiento
                });
            }
            return new AporteRecetaLiquidaDto
            {
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MaxValue,
                Total = total,
                Valores = lista
            };
        }
        private List<FacturaAcumuladaDto> CompleteMissedInvoices(List<FacturaAcumuladaDto> facturas, DateTime startDate, DateTime endDate)
        {
            var listOfMonth = _dateTimeService.GetDateTimesInBetween(startDate, endDate);
            if (facturas == null)
                facturas = new List<FacturaAcumuladaDto>();
            foreach (var item in listOfMonth)
            {
                if (facturas.Any(f => f.Mes.Year == item.Year && f.Mes.Month == item.Month))
                    continue;
                facturas.Add(new FacturaAcumuladaDto { Mes = item, RecetaLiquida = 0.0 });
            }
            facturas = facturas.OrderBy(f => f.Mes).ToList();
            return facturas;
        }
        public List<UsuarioDto> GetGraphic(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        {
            ArgumentNullException.ThrowIfNull(startDate, nameof(startDate));
            ArgumentNullException.ThrowIfNull(endDate, nameof(endDate));
            ArgumentNullException.ThrowIfNull(coUsuarios, nameof(coUsuarios));
            var listOfUserCodes = coUsuarios.GetAsCsvSingleQuote();
            var allUserCodes = coUsuarios?.ToList() ?? new List<string>();
            var query = "select u.co_usuario, u.no_usuario, f.data_emissao, sum(f.valor) as valor, sum(f.valor - (f.valor * f.total_imp_inc/100)) as receita_liquida, s.brut_salario, " +
                "CONCAT(year(f.data_emissao), '-', month(f.data_emissao)) as yearmonth " +
                "from cao_fatura as f " +
                "join cao_os as o on f.co_os = o.co_os join cao_usuario as u on o.co_usuario = u.co_usuario join cao_salario as s on u.co_usuario = s.co_usuario " +
                "where f.data_emissao between {0} and {1} and locate (u.co_usuario, {2}) > 0 " +
                "group by u.co_usuario, yearmonth " +
                "order by u.no_usuario, f.data_emissao";
            var queryResult = _CaoFaturaRepository.UnitOfWork
                .ExecuteQuery<UsuarioPerformance>(query, new object[]
                {
                    _dateTimeService.ParsedAsMySql( startDate.Value),
                    _dateTimeService.ParsedAsMySql( endDate.Value),
                    listOfUserCodes
                }).ToList();
            var queryUsers = _caoUsuarioAppService.GetUsuarios(coUsuarios);
            var listOfUsers = new List<UsuarioDto>();
            foreach (var item in allUserCodes)
            {
                var aportes = queryResult.Where(a => a.CoUsuario == item).ToList() ?? new List<UsuarioPerformance>();
                var aporteUsuario = aportes.FirstOrDefault(a => a.CoUsuario == item);

                var usuario = aporteUsuario == null
                ? queryUsers.FirstOrDefault(u => u.Code == item)
                : new UsuarioDto()
                {
                    Code = item,
                    Name = aporteUsuario?.NoUsuario ?? item,
                    BrutSalario = aporteUsuario?.BrutSalario ?? 0.0
                };
                if (usuario == null)
                    continue;
                var facturas = new List<FacturaAcumuladaDto>();
                foreach (var aporte in aportes)
                {
                    facturas.Add(new FacturaAcumuladaDto
                    {
                        Mes = aporte.DataEmissao,
                        Valor = aporte.Valor,
                        RecetaLiquida = aporte.ReceitaLiquida
                    });
                }
                facturas = CompleteMissedInvoices(facturas, startDate.Value, endDate.Value);
                usuario.Facturas = facturas;
                listOfUsers.Add(usuario);
            }
            return listOfUsers;
        }
        public List<UsuarioDto> GetRelatorio(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        {
            ArgumentNullException.ThrowIfNull(startDate, nameof(startDate));
            ArgumentNullException.ThrowIfNull(endDate, nameof(endDate));
            ArgumentNullException.ThrowIfNull(coUsuarios, nameof(coUsuarios));
            var listOfUserCodes = coUsuarios.GetAsCsvSingleQuote();
            var allUserCodes = coUsuarios?.ToList() ?? new List<string>();
            var query = "select  u.co_usuario, u.no_usuario, f.data_emissao, sum(f.valor) as valor, sum(f.valor - (f.valor*f.total_imp_inc/100)) as receita_liquida, s.brut_salario, sum( (f.valor - (f.valor * f.total_imp_inc/100)) * f.comissao_cn / 100  ) as comissao , f.comissao_cn, sum( (f.valor - (f.valor*f.total_imp_inc/100)) - ( (f.valor - (f.valor * f.total_imp_inc/100)) * f.comissao_cn / 100)) - s.brut_salario as lucro, " +
                "CONCAT(year(f.data_emissao), '-', month(f.data_emissao)) as yearmonth " +
                "from cao_fatura as f " +
                "join cao_os as o on f.co_os = o.co_os join cao_usuario as u on o.co_usuario = u.co_usuario join cao_salario as s on u.co_usuario = s.co_usuario " +
                "where f.data_emissao between {0} and {1} and locate (u.co_usuario, {2}) > 0 " +
                "group by u.co_usuario, yearmonth " +
                "order by u.no_usuario, f.data_emissao";
            var queryResult = _CaoFaturaRepository.UnitOfWork
                .ExecuteQuery<UsuarioRelatorio>(query, new object[]
                {
                    _dateTimeService.ParsedAsMySql( startDate.Value),
                    _dateTimeService.ParsedAsMySql( endDate.Value),
                    listOfUserCodes
                }).ToList();
            var listOfMonth = _dateTimeService.GetDateTimesInBetween(startDate.Value, endDate.Value);
            var queryUsers = _caoUsuarioAppService.GetUsuarios(coUsuarios);
            var listOfUsers = new List<UsuarioDto>();
            foreach (var item in allUserCodes)
            {
                var aportes = queryResult.Where(a => a.CoUsuario == item).ToList() ?? new List<UsuarioRelatorio>(); ;
                var aporteUsuario = aportes.FirstOrDefault(a => a.CoUsuario == item);
                var usuario = aporteUsuario == null
                    ? queryUsers.FirstOrDefault(u => u.Code == item)
                    : new UsuarioDto()
                    {
                        Code = aporteUsuario?.CoUsuario ?? item,
                        Name = aporteUsuario?.NoUsuario ?? item,
                        BrutSalario = aporteUsuario?.BrutSalario ?? 0.0
                    };
                if (usuario == null)
                    continue;
                var facturas = new List<FacturaAcumuladaDto>();
                foreach (var aporte in aportes)
                {
                    facturas.Add(new FacturaAcumuladaDto { Mes = aporte.DataEmissao, Comissao = aporte.Comissao, RecetaLiquida = aporte.ReceitaLiquida, Valor = aporte.Valor, Lucro = aporte.Lucro });
                }
                facturas = CompleteMissedInvoices(facturas, startDate.Value, endDate.Value);
                usuario.Facturas = facturas;
                listOfUsers.Add(usuario);
            }
            return listOfUsers;
        }
    }
}
