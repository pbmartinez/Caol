using Domain.Entities;
using Domain.Utils;
using Domain.Specification;
using Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }
        TEntity Get(string id, List<string> includes = null);
        Task<IQueryable<TEntity>> GetAllAsync(List<string> includes = null, Dictionary<string, bool> order = null);
        Task<TEntity> GetAsync(string id, List<string> includes = null);
        Task AddAsync(TEntity item);
        Task UpdateAsync(TEntity item);
        Task DeleteAsync(TEntity item);

        //All Searches by Specification Pattern in Service layer is translated in searches by expression in Repostory Layer
        //FindOneBySpecificationPatternAsync -> FindOneByExpressionAsync
        //The expression that is defined in the specification
        
        Task<TEntity> FindOneByExpressionAsync(Expression<Func<TEntity,bool>> expression, List<string> includes);
        Task<PagedCollection<TEntity>> FindPageByExpressionAsync(Expression<Func<TEntity, bool>> expression, List<string> includes, Dictionary<string, bool> order, int pageSize, int pageGo);
        Task<List<TEntity>> FindAllByExpressionAsync(Expression<Func<TEntity, bool>> expression, List<string> includes = null, Dictionary<string, bool> order = null);
        Task<int> FindCountByExpressionAsync(Expression<Func<TEntity, bool>> expression);
    }
}
