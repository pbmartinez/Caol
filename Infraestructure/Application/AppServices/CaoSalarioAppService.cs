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
    public partial class CaoSalarioAppService : ICaoSalarioAppService
    {
        private readonly ICaoSalarioRepository _CaoSalarioRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public CaoSalarioAppService(ICaoSalarioRepository CaoSalarioRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _CaoSalarioRepository = CaoSalarioRepository ?? throw new ArgumentNullException(nameof(CaoSalarioRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(CaoSalarioDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoSalarioRepository.AddAsync(_mapper.Map<CaoSalario>(item));
                commited = await _CaoSalarioRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }
        
        public async Task<List<CaoSalarioDto>> FindAllBySpecificationPatternAsync(Specification<CaoSalarioDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<CaoSalarioDto>>(
                await _CaoSalarioRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<CaoSalario, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<CaoSalarioDto>? specification = null)
        {
            var count = await _CaoSalarioRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<CaoSalario>());
            return count;
        }

        public async Task<CaoSalarioDto> FindOneBySpecificationPatternAsync(Specification<CaoSalarioDto>? specification = null, List<string>? includes = null)
        {
            var item = await _CaoSalarioRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<CaoSalario>(), includes);
            return _mapper.Map<CaoSalarioDto>(item);
        }

        public async Task<List<CaoSalarioDto>> FindPageBySpecificationPatternAsync(Specification<CaoSalarioDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<CaoSalarioDto>>(
                await _CaoSalarioRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<CaoSalario>(), includes, order, pageSize, pageGo));
        }

        
        public CaoSalarioDto Get(string id, List<string>? includes = null)
        {
            return _mapper.Map<CaoSalarioDto>(_CaoSalarioRepository.Get(id, includes));
        }

        public async Task<List<CaoSalarioDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _CaoSalarioRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<CaoSalarioDto>>(items.ToList());
            return dtoItems;
        }

        public async Task<CaoSalarioDto> GetAsync(string id, List<string>? includes = null)
        {
            return _mapper.Map<CaoSalarioDto>(await _CaoSalarioRepository.GetAsync(id, includes));
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var item = await _CaoSalarioRepository.GetAsync(id);
            await _CaoSalarioRepository.DeleteAsync(item);
            var commited = await _CaoSalarioRepository.UnitOfWork.CommitAsync();

            return commited > 0;
        }

        public async Task<bool> UpdateAsync(CaoSalarioDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoSalarioRepository.UpdateAsync(_mapper.Map<CaoSalario>(item));
                commited = await _CaoSalarioRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

    }
}
