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
    public partial class CaoOAppService : ICaoOAppService
    {
        private readonly ICaoORepository _CaoORepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public CaoOAppService(ICaoORepository CaoORepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _CaoORepository = CaoORepository ?? throw new ArgumentNullException(nameof(CaoORepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(CaoODto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoORepository.AddAsync(_mapper.Map<CaoO>(item));
                commited = await _CaoORepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

        
        public async Task<List<CaoODto>> FindAllBySpecificationPatternAsync(Specification<CaoODto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<CaoODto>>(
                   await _CaoORepository.FindAllByExpressionAsync(
                       _mapper.MapExpression<Expression<Func<CaoO, bool>>>(
                           specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<CaoODto>? specification = null)
        {
            var count = await _CaoORepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<CaoO>());
            return count;
        }

       

        public async Task<CaoODto> FindOneBySpecificationPatternAsync(Specification<CaoODto>? specification = null, List<string>? includes = null)
        {
            var item = await _CaoORepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<CaoO>(), includes);
            return _mapper.Map<CaoODto>(item);
        }

        

        public async Task<List<CaoODto>> FindPageBySpecificationPatternAsync(Specification<CaoODto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<CaoODto>>(
                await _CaoORepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<CaoO>(), includes, order, pageSize, pageGo));
        }

        

        //public CaoODto Get(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoODto>(_CaoORepository.Get(id, includes));
        //}


        public async Task<List<CaoODto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _CaoORepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<CaoODto>>(items.ToList());
            return dtoItems;
        }


        //public async Task<CaoODto> GetAsync(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoODto>(await _CaoORepository.GetAsync(id, includes));
        //}


        //public async Task<bool> RemoveAsync(string id)
        //{
        //    var item = await _CaoORepository.GetAsync(id);
        //    await _CaoORepository.DeleteAsync(item);
        //    var commited = await _CaoORepository.UnitOfWork.CommitAsync();

        //    return commited > 0;
        //}

        //public async Task<bool> UpdateAsync(CaoODto item)
        //{
        //    int commited;
        //    if (_entityValidator.IsValid(item))
        //    {
        //        await _CaoORepository.UpdateAsync(_mapper.Map<CaoO>(item));
        //        commited = await _CaoORepository.UnitOfWork.CommitAsync();
        //    }
        //    else
        //        throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
        //    return commited > 0;
        //}

    }
}
