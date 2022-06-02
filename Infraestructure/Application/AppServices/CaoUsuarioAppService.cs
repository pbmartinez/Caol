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
    public partial class CaoUsuarioAppService : ICaoUsuarioAppService
    {
        private readonly ICaoUsuarioRepository _CaoUsuarioRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public CaoUsuarioAppService(ICaoUsuarioRepository CaoUsuarioRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _CaoUsuarioRepository = CaoUsuarioRepository ?? throw new ArgumentNullException(nameof(CaoUsuarioRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(CaoUsuarioDto item)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _CaoUsuarioRepository.AddAsync(_mapper.Map<CaoUsuario>(item));
                commited = await _CaoUsuarioRepository.UnitOfWork.CommitAsync();
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }
        
        public async Task<List<CaoUsuarioDto>> FindAllBySpecificationPatternAsync(Specification<CaoUsuarioDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            return _mapper.Map<List<CaoUsuarioDto>>(
                await _CaoUsuarioRepository.FindAllByExpressionAsync(
                    _mapper.MapExpression<Expression<Func<CaoUsuario, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<CaoUsuarioDto>? specification = null)
        {
            var count = await _CaoUsuarioRepository.FindCountByExpressionAsync(specification?.MapToExpressionOfType<CaoUsuario>());
            return count;
        }

        public async Task<CaoUsuarioDto> FindOneBySpecificationPatternAsync(Specification<CaoUsuarioDto>? specification = null, List<string>? includes = null)
        {
            var item = await _CaoUsuarioRepository.FindOneByExpressionAsync(specification?.MapToExpressionOfType<CaoUsuario>(), includes);
            return _mapper.Map<CaoUsuarioDto>(item);
        }

        public async Task<List<CaoUsuarioDto>> FindPageBySpecificationPatternAsync(Specification<CaoUsuarioDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0)
        {
            return _mapper.Map<List<CaoUsuarioDto>>(
                await _CaoUsuarioRepository.FindPageByExpressionAsync(
                    specification?.MapToExpressionOfType<CaoUsuario>(), includes, order, pageSize, pageGo));
        }

        
        //public CaoUsuarioDto Get(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoUsuarioDto>(_CaoUsuarioRepository.Get(id, includes));
        //}

        public async Task<List<CaoUsuarioDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _CaoUsuarioRepository.GetAllAsync(includes, order);
            var dtoItems = _mapper.Map<List<CaoUsuarioDto>>(items.ToList());
            return dtoItems;
        }

        //public async Task<CaoUsuarioDto> GetAsync(string id, List<string>? includes = null)
        //{
        //    return _mapper.Map<CaoUsuarioDto>(await _CaoUsuarioRepository.GetAsync(id, includes));
        //}

        //public async Task<bool> RemoveAsync(string id)
        //{
        //    var item = await _CaoUsuarioRepository.GetAsync(id);
        //    await _CaoUsuarioRepository.DeleteAsync(item);
        //    var commited = await _CaoUsuarioRepository.UnitOfWork.CommitAsync();

        //    return commited > 0;
        //}

        //public async Task<bool> UpdateAsync(CaoUsuarioDto item)
        //{
        //    int commited;
        //    if (_entityValidator.IsValid(item))
        //    {
        //        await _CaoUsuarioRepository.UpdateAsync(_mapper.Map<CaoUsuario>(item));
        //        commited = await _CaoUsuarioRepository.UnitOfWork.CommitAsync();
        //    }
        //    else
        //        throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
        //    return commited > 0;
        //}

    }
}
