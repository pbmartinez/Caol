using Domain.Entities;
using Domain.Utils;
using Domain.IRepositories;
using Domain.Specification;
using Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Domain.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IUnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public async Task AddAsync(TEntity item)
        {
            await _unitOfWork.Repository<TEntity>().AddAsync(item);
        }

        public async Task DeleteAsync(TEntity item)
        {
            await Task.FromResult(_unitOfWork.Repository<TEntity>().Remove(item));
        }

        

        //public async Task UpdateAsync(TEntity item)
        //{
        //    var a = await GetAsync(item.Id);
        //    _unitOfWork.GetEntry(a).CurrentValues.SetValues(item);
        //    await Task.CompletedTask;
        //}



        public async Task<int> FindCountByExpressionAsync(Expression<Func<TEntity, bool>>? expression)
        {
            var count = await _unitOfWork.GetQueryable(null, expression).CountAsync();
            return count;
        }


        //public TEntity Get(string id, List<string>? includes = null)
        //{
        //    var item = _unitOfWork.GetQueryable(includes,(Expression<Func<TEntity,bool>>) (a => a.Id == id)).FirstOrDefault();
        //    return item;
        //}

        public async Task<IQueryable<TEntity>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = _unitOfWork.GetQueryable(includes, (Expression<Func<TEntity, bool>>)(a => true), order,0,0);
            return (IQueryable<TEntity>)await Task.FromResult(items);
        }

        //public async Task<TEntity> GetAsync(string id, List<string>? includes = null)
        //{
        //    var i = includes == null ? new List<string>() : includes.ToList();
        //    var info = typeof(TEntity);
        //    var attr = typeof(TEntity).Attributes;
            
        //    var item = await _unitOfWork.GetQueryable(i, (Expression<Func<TEntity, bool>>)(a => a.Id == id)).FirstOrDefaultAsync();
        //    return item;
        //}

        public async Task<TEntity> FindOneByExpressionAsync(Expression<Func<TEntity, bool>>? expression, List<string>? includes)
        {
            var item = await _unitOfWork.GetQueryable(includes, expression).FirstOrDefaultAsync();
            return item;
        }

        public async Task<PagedCollection<TEntity>> FindPageByExpressionAsync(Expression<Func<TEntity, bool>>? expression, List<string>? includes, Dictionary<string, bool>? order, int pageSize, int pageGo)
        {
            if (order == null || order.Count == 0)
                order = new Dictionary<string, bool>() { { "Id", true } };
            var items = await _unitOfWork.GetQueryable(includes, expression, order, pageSize, pageGo).ToListAsync();
            var totalItems = await _unitOfWork.GetQueryable(null, expression).CountAsync();

            var page = new PagedCollection<TEntity>()
            {
                Items = items,
                Order = order,
                PageIndex = pageGo,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return page;
        }

        public async Task<List<TEntity>> FindAllByExpressionAsync(Expression<Func<TEntity, bool>>? expression, List<string>? includes = null, Dictionary<string, bool>? order = null)
        {
            var items = await _unitOfWork.GetQueryable(includes, expression, order).ToListAsync();
            return items;
        }
    }
}
