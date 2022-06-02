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
    public partial class CaoClienteAppService : ICaoClienteAppService
    {
        private readonly ICaoClienteRepository _CaoClienteRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public CaoClienteAppService(ICaoClienteRepository CaoClienteRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _CaoClienteRepository = CaoClienteRepository ?? throw new ArgumentNullException(nameof(CaoClienteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(CaoClienteDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoClienteRepository.AddAsync(_mapper.Map<CaoCliente>(item));
                commited = await _CaoClienteRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }
        
        public async Task<List<CaoClienteDto>> FindAllBySpecificationPatternAsync(Specification<CaoClienteDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<CaoClienteDto>>(
                await _CaoClienteRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<CaoCliente, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<CaoClienteDto>? specification = null)
        {
            var count = await _CaoClienteRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<CaoCliente>());
            return count;
        }

        public async Task<CaoClienteDto> FindOneBySpecificationPatternAsync(Specification<CaoClienteDto>? specification = null, List<string>? includes = null)
        {
            var item = await _CaoClienteRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<CaoCliente>(), includes);
            return _mapper.Map<CaoClienteDto>(item);
        }

        public async Task<List<CaoClienteDto>> FindPageBySpecificationPatternAsync(Specification<CaoClienteDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<CaoClienteDto>>(
                await _CaoClienteRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<CaoCliente>(), includes, order, pageSize, pageGo));
        }

        
        public CaoClienteDto Get(string id, List<string>? includes = null)
        {
            return _mapper.Map<CaoClienteDto>(_CaoClienteRepository.Get(id, includes));
        }

        public async Task<List<CaoClienteDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _CaoClienteRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<CaoClienteDto>>(items.ToList());
            return dtoItems;
        }

        public async Task<CaoClienteDto> GetAsync(string id, List<string>? includes = null)
        {
            return _mapper.Map<CaoClienteDto>(await _CaoClienteRepository.GetAsync(id, includes));
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var item = await _CaoClienteRepository.GetAsync(id);
            await _CaoClienteRepository.DeleteAsync(item);
            var commited = await _CaoClienteRepository.UnitOfWork.CommitAsync();

            return commited > 0;
        }

        public async Task<bool> UpdateAsync(CaoClienteDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoClienteRepository.UpdateAsync(_mapper.Map<CaoCliente>(item));
                commited = await _CaoClienteRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

    }
}
