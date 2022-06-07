﻿using Application.Dtos;
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

namespace Infraestructure.Application.AppServices
{
    public partial class CaoFaturaAppService : ICaoFaturaAppService
    {
        private readonly ICaoFaturaRepository _CaoFaturaRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public CaoFaturaAppService(ICaoFaturaRepository CaoFaturaRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _CaoFaturaRepository = CaoFaturaRepository ?? throw new ArgumentNullException(nameof(CaoFaturaRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
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
        public async Task<AporteRecetaLiquidaDto> GetPizzaAsync(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        {
            var facturas = await GetFacturasAsync(startDate, endDate, coUsuarios);
            var facturasAgrupadasPorUsuario = facturas
                .GroupBy(f => f.CaoOrdenServicio.CaoUsuario)
                .Select(u =>
                new
                {
                    coUsuario = u.Key.CoUsuario,
                    recetaLiquida = u.Key.CaoOrdenesServicios
                                 .Aggregate(0.0, (a, b) => b.CaoFaturas
                                 .Aggregate(0.0, (s, f) => f.ReceitaLiquida + s))
                }).ToList();

            var total = facturasAgrupadasPorUsuario
                .Sum(u=>u.recetaLiquida);
            var lista = new List<(string, double, double)>();
            foreach (var usuario in facturasAgrupadasPorUsuario)
            {
                var porCiento = total > 0 ? usuario.recetaLiquida / total * 100 : 0.0;
                lista.Add(new ( usuario.coUsuario,usuario.recetaLiquida, porCiento ));
            }
            var aportes = new AporteRecetaLiquidaDto
            {
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MaxValue,
                Total = total,
                Valores = lista
            };

            return aportes;
        }
        public async Task<AporteRecetaLiquidaDto> GetGraficoAsync(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        {
            var facturas = await GetFacturasAsync(startDate, endDate, coUsuarios);
            var facturasAgrupadasPorUsuario = facturas
                .GroupBy(f => f.CaoOrdenServicio.CaoUsuario)
                .Select(u =>
                new
                {
                    coUsuario = u.Key.CoUsuario,
                    salario = u.Key.CaoSalario.BrutSalario,
                    recetaLiquida = u.Key.CaoOrdenesServicios
                                 .Aggregate(0.0, (a, b) => b.CaoFaturas
                                 .Aggregate(0.0, (s, f) => f.ReceitaLiquida + s))

                }).ToList();

            var total = facturasAgrupadasPorUsuario
                .Sum(u=>u.recetaLiquida);
            var lista = new List<(string, double, double)>();
            foreach (var usuario in facturasAgrupadasPorUsuario)
            {
                var porCiento = total > 0 ? usuario.recetaLiquida / total * 100 : 0.0;
                lista.Add(new ( usuario.coUsuario,usuario.recetaLiquida, porCiento ));
            }
            var aportes = new AporteRecetaLiquidaDto
            {
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MaxValue,
                Total = total,
                Valores = lista
            };

            return aportes;
        }


        //public async Task<List<dynamic>> GetRelatorioAsync(DateTime? startDate, DateTime? endDate, IEnumerable<string>? coUsuarios)
        //{
        //    var facturas = await GetFacturasAsync(startDate, endDate, coUsuarios);

        //    // ToDo  GroupBy => data emissao
        //    var g = facturas
        //        .GroupBy(f => new { f.CaoOrdenServicio.CaoUsuario, f.DataEmissao })
        //        .Select(u =>
        //        new
        //        {
        //            coUsuario = u.Key.CaoUsuario,
        //            brutSalario = u.Key.CaoUsuario.CaoSalario.BrutSalario,
        //            valor = u.Key.CaoUsuario.CaoOrdenesServicios
        //                         .Aggregate(0.0, (a, b) => b.CaoFaturas
        //                          .Aggregate(0.0, (s, f) => f.Valor + s)),
        //            recetaLiquida = u.Key.CaoUsuario.CaoOrdenesServicios
        //                         .Aggregate(0.0, (a, b) => b.CaoFaturas
        //                          .Aggregate(0.0, (s, f) => f.ReceitaLiquida + s)),
        //            comissao = u.Key.CaoUsuario.CaoOrdenesServicios
        //                         .Aggregate(0.0, (a, b) => b.CaoFaturas
        //                          .Aggregate(0.0, (s, f) => f.Comissao + s)),
        //            lucro = u.Key.CaoUsuario.CaoOrdenesServicios
        //                         .Aggregate(0.0, (a, b) => b.CaoFaturas
        //                          .Aggregate(0.0, (s, f) => f.Lucro + s))

        //        }).ToList();
        //    return g;
        //}

        public async Task<List<CaoFaturaDto>> GetRelatorioAsync(DateTime? startDate, DateTime? endDate, IEnumerable<string> coUsuarios)
        {
            throw new NotImplementedException();
        }
    }
}
