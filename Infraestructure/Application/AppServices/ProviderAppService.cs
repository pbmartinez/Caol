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
    public partial class ProviderAppService : ICaoClienteAppService
    {
        private readonly IProviderRepository _ProviderRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public ProviderAppService(IProviderRepository ProviderRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _ProviderRepository = ProviderRepository ?? throw new ArgumentNullException(nameof(ProviderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(ProviderDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _ProviderRepository.AddAsync(_mapper.Map<Provider>(item));
                commited = await _ProviderRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }
        
        public async Task<List<ProviderDto>> FindAllBySpecificationPatternAsync(Specification<ProviderDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<ProviderDto>>(
                await _ProviderRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<Provider, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<ProviderDto>? specification = null)
        {
            var count = await _ProviderRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<Provider>());
            return count;
        }

        public async Task<ProviderDto> FindOneBySpecificationPatternAsync(Specification<ProviderDto>? specification = null, List<string>? includes = null)
        {
            var item = await _ProviderRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<Provider>(), includes);
            return _mapper.Map<ProviderDto>(item);
        }

        public async Task<List<ProviderDto>> FindPageBySpecificationPatternAsync(Specification<ProviderDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<ProviderDto>>(
                await _ProviderRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<Provider>(), includes, order, pageSize, pageGo));
        }

        
        public ProviderDto Get(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<ProviderDto>(_ProviderRepository.Get(id, includes));
        }

        public async Task<List<ProviderDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _ProviderRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<ProviderDto>>(items.ToList());
            return dtoItems;
        }

        public async Task<ProviderDto> GetAsync(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<ProviderDto>(await _ProviderRepository.GetAsync(id, includes));
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var item = await _ProviderRepository.GetAsync(id);
            await _ProviderRepository.DeleteAsync(item);
            var commited = await _ProviderRepository.UnitOfWork.CommitAsync();

            return commited > 0;
        }

        public async Task<bool> UpdateAsync(ProviderDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _ProviderRepository.UpdateAsync(_mapper.Map<Provider>(item));
                commited = await _ProviderRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

    }
}
