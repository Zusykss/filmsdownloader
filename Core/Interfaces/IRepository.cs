using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Classes;

namespace Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        PagedList<TEntity> GetByPage(QueryStringParameters queryStringParameters);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> GetById(object id);
        Task Insert(TEntity entity);
        Task Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task SaveChangesAsync();
    }
}
