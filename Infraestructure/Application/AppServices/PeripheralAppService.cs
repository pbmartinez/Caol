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
    public partial class PeripheralAppService : ICaoFaturaAppService
    {
        private readonly IPeripheralRepository _PeripheralRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public PeripheralAppService(IPeripheralRepository peripheralRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _PeripheralRepository = peripheralRepository ?? throw new ArgumentNullException(nameof(peripheralRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(PeripheralDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _PeripheralRepository.AddAsync(_mapper.Map<Peripheral>(item));
                commited = await _PeripheralRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

        
        public async Task<List<PeripheralDto>> FindAllBySpecificationPatternAsync(Specification<PeripheralDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<PeripheralDto>>(
                   await _PeripheralRepository.FindAllByExpressionAsync(
                       _mapper.MapExpression<Expression<Func<Peripheral, bool>>>(
                           specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<PeripheralDto>? specification = null)
        {
            var count = await _PeripheralRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<Peripheral>());
            return count;
        }

       

        public async Task<PeripheralDto> FindOneBySpecificationPatternAsync(Specification<PeripheralDto>? specification = null, List<string>? includes = null)
        {
            var item = await _PeripheralRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<Peripheral>(), includes);
            return _mapper.Map<PeripheralDto>(item);
        }

        

        public async Task<List<PeripheralDto>> FindPageBySpecificationPatternAsync(Specification<PeripheralDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<PeripheralDto>>(
                await _PeripheralRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<Peripheral>(), includes, order, pageSize, pageGo));
        }

        

        public PeripheralDto Get(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<PeripheralDto>(_PeripheralRepository.Get(id, includes));
        }


        public async Task<List<PeripheralDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _PeripheralRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<PeripheralDto>>(items.ToList());
            return dtoItems;
        }


        public async Task<PeripheralDto> GetAsync(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<PeripheralDto>(await _PeripheralRepository.GetAsync(id, includes));
        }


        public async Task<bool> RemoveAsync(Guid id)
        {
            var item = await _PeripheralRepository.GetAsync(id);
            await _PeripheralRepository.DeleteAsync(item);
            var commited = await _PeripheralRepository.UnitOfWork.CommitAsync();

            return commited > 0;
        }

        public async Task<bool> UpdateAsync(PeripheralDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _PeripheralRepository.UpdateAsync(_mapper.Map<Peripheral>(item));
                commited = await _PeripheralRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

    }
}
