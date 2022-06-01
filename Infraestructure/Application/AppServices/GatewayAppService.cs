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
    public partial class GatewayAppService : IPermissaoSistemaAppService
    {
        private readonly IGatewayRepository _GatewayRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public GatewayAppService(IGatewayRepository gatewayRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _GatewayRepository = gatewayRepository ?? throw new ArgumentNullException(nameof(gatewayRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(GatewayDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _GatewayRepository.AddAsync(_mapper.Map<Gateway>(item));
                commited = await _GatewayRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

        
        public async Task<List<GatewayDto>> FindAllBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<GatewayDto>>(
                await _GatewayRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<Gateway, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<GatewayDto>? specification = null)
        {
            var count = await _GatewayRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<Gateway>());
            return count;
        }

        public async Task<GatewayDto> FindOneBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, List<string>? includes = null)
        {
            var item = await _GatewayRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<Gateway>(), includes);
            return _mapper.Map<GatewayDto>(item);
        }

        public async Task<List<GatewayDto>> FindPageBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<GatewayDto>>(
                await _GatewayRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<Gateway>(), includes, order, pageSize, pageGo));
        }


        public GatewayDto Get(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<GatewayDto>(_GatewayRepository.Get(id, includes));
        }

        public async Task<List<GatewayDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _GatewayRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<GatewayDto>>(items.ToList());
            return dtoItems;
        }


        public async Task<GatewayDto> GetAsync(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<GatewayDto>(await _GatewayRepository.GetAsync(id, includes));
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var item = await _GatewayRepository.GetAsync(id);
            await _GatewayRepository.DeleteAsync(item);
            var commited = await _GatewayRepository.UnitOfWork.CommitAsync();

            return commited > 0;
        }

        public async Task<bool> UpdateAsync(GatewayDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _GatewayRepository.UpdateAsync(_mapper.Map<Gateway>(item));
                commited = await _GatewayRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

    }
}
