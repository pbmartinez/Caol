using Application.IValidator;
using Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IAppServices
{
    public interface IAppService<TEntity>
    {
        TEntity Get(string id, List<string> includes = null);
        Task<bool> AddAsync(TEntity item);
        Task<bool> UpdateAsync(TEntity item);
        Task<TEntity> GetAsync(string id, List<string> includes = null);
        Task<List<TEntity>> GetAllAsync(List<string> includes = null, Dictionary<string, bool> order = null);
        Task<bool> RemoveAsync(string id);
        Task<TEntity> FindOneBySpecificationPatternAsync(Specification<TEntity> specification = null, List<string> includes = null);
        Task<List<TEntity>> FindPageBySpecificationPatternAsync(Specification<TEntity> specification = null, List<string> includes = null, Dictionary<string, bool> order = null, int pageSize = 0, int pageGo = 0);
        Task<List<TEntity>> FindAllBySpecificationPatternAsync(Specification<TEntity> specification = null, List<string> includes = null, Dictionary<string, bool> order = null);
        Task<int> FindCountBySpecificationPatternAsync(Specification<TEntity> specification = null);
    }
}
