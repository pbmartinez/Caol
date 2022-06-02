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
    public partial class PermissaoSistemaAppService : IPermissaoSistemaAppService
    {
        private readonly IPermissaoSistemaRepository _PermissaoSistemaRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public PermissaoSistemaAppService(IPermissaoSistemaRepository PermissaoSistemaRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _PermissaoSistemaRepository = PermissaoSistemaRepository ?? throw new ArgumentNullException(nameof(PermissaoSistemaRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(PermissaoSistemaDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _PermissaoSistemaRepository.AddAsync(_mapper.Map<PermissaoSistema>(item));
                commited = await _PermissaoSistemaRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }
        
        public async Task<List<PermissaoSistemaDto>> FindAllBySpecificationPatternAsync(Specification<PermissaoSistemaDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<PermissaoSistemaDto>>(
                await _PermissaoSistemaRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<PermissaoSistema, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<PermissaoSistemaDto>? specification = null)
        {
            var count = await _PermissaoSistemaRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<PermissaoSistema>());
            return count;
        }

        public async Task<PermissaoSistemaDto> FindOneBySpecificationPatternAsync(Specification<PermissaoSistemaDto>? specification = null, List<string>? includes = null)
        {
            var item = await _PermissaoSistemaRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<PermissaoSistema>(), includes);
            return _mapper.Map<PermissaoSistemaDto>(item);
        }

        public async Task<List<PermissaoSistemaDto>> FindPageBySpecificationPatternAsync(Specification<PermissaoSistemaDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<PermissaoSistemaDto>>(
                await _PermissaoSistemaRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<PermissaoSistema>(), includes, order, pageSize, pageGo));
        }

        
        //public PermissaoSistemaDto Get(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<PermissaoSistemaDto>(_PermissaoSistemaRepository.Get(id, includes));
        //}

        public async Task<List<PermissaoSistemaDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _PermissaoSistemaRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<PermissaoSistemaDto>>(items.ToList());
            return dtoItems;
        }

        //public async Task<PermissaoSistemaDto> GetAsync(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<PermissaoSistemaDto>(await _PermissaoSistemaRepository.GetAsync(id, includes));
        //}

        //public async Task<bool> RemoveAsync(string id)
        //{
        //    var item = await _PermissaoSistemaRepository.GetAsync(id);
        //    await _PermissaoSistemaRepository.DeleteAsync(item);
        //    var commited = await _PermissaoSistemaRepository.UnitOfWork.CommitAsync();

        //    return commited > 0;
        //}

        //public async Task<bool> UpdateAsync(PermissaoSistemaDto item)
        //{
        //    int commited;
        //    if (_entityValidator.IsValid(item))
        //    {
        //        await _PermissaoSistemaRepository.UpdateAsync(_mapper.Map<PermissaoSistema>(item));
        //        commited = await _PermissaoSistemaRepository.UnitOfWork.CommitAsync();
        //    }
        //    else
        //        throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
        //    return commited > 0;
        //}

    }
}
