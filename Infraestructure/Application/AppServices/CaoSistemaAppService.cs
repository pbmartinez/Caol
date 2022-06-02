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
    public partial class CaoSistemaAppService : ICaoSistemaAppService
    {
        private readonly ICaoSistemaRepository _CaoSistemaRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public CaoSistemaAppService(ICaoSistemaRepository CaoSistemaRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _CaoSistemaRepository = CaoSistemaRepository ?? throw new ArgumentNullException(nameof(CaoSistemaRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(CaoSistemaDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoSistemaRepository.AddAsync(_mapper.Map<CaoSistema>(item));
                commited = await _CaoSistemaRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }
        
        public async Task<List<CaoSistemaDto>> FindAllBySpecificationPatternAsync(Specification<CaoSistemaDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<CaoSistemaDto>>(
                await _CaoSistemaRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<CaoSistema, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<CaoSistemaDto>? specification = null)
        {
            var count = await _CaoSistemaRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<CaoSistema>());
            return count;
        }

        public async Task<CaoSistemaDto> FindOneBySpecificationPatternAsync(Specification<CaoSistemaDto>? specification = null, List<string>? includes = null)
        {
            var item = await _CaoSistemaRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<CaoSistema>(), includes);
            return _mapper.Map<CaoSistemaDto>(item);
        }

        public async Task<List<CaoSistemaDto>> FindPageBySpecificationPatternAsync(Specification<CaoSistemaDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<CaoSistemaDto>>(
                await _CaoSistemaRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<CaoSistema>(), includes, order, pageSize, pageGo));
        }

        
        //public CaoSistemaDto Get(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoSistemaDto>(_CaoSistemaRepository.Get(id, includes));
        //}

        public async Task<List<CaoSistemaDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _CaoSistemaRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<CaoSistemaDto>>(items.ToList());
            return dtoItems;
        }

        //public async Task<CaoSistemaDto> GetAsync(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoSistemaDto>(await _CaoSistemaRepository.GetAsync(id, includes));
        //}

        //public async Task<bool> RemoveAsync(string id)
        //{
        //    var item = await _CaoSistemaRepository.GetAsync(id);
        //    await _CaoSistemaRepository.DeleteAsync(item);
        //    var commited = await _CaoSistemaRepository.UnitOfWork.CommitAsync();

        //    return commited > 0;
        //}

        //public async Task<bool> UpdateAsync(CaoSistemaDto item)
        //{
        //    int commited;
        //    if (_entityValidator.IsValid(item))
        //    {
        //        await _CaoSistemaRepository.UpdateAsync(_mapper.Map<CaoSistema>(item));
        //        commited = await _CaoSistemaRepository.UnitOfWork.CommitAsync();
        //    }
        //    else
        //        throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
        //    return commited > 0;
        //}

    }
}
