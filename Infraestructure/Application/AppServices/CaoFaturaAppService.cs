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

        public async Task<List<CaoFaturaDto>> GetRelatorioAsync(DateTime? startDate, DateTime? endDate, List<string>? coUsuarios)
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
            
            
            Expression<Func<CaoFatura, bool>> expTrueForAll =
                f => true;
            var includes = new List<string>()
            {
                $"{nameof(CaoFatura.CaoOrdenServicio)}",
                $"{nameof(CaoFatura.CaoOrdenServicio)}.{nameof(CaoO.CaoUsuario)}"
            };

            var facturas = await _CaoFaturaRepository.FindAllByExpressionAsync(expTrueForAll,includes);

            return _mapper.Map<List<CaoFaturaDto>>(facturas.ToList());
        }
    }
}
