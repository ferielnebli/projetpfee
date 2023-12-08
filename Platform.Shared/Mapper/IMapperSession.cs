using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Mapper
{
    public interface IMapperSession<T>
    {
        void BeginTransaction();
        Task Commit();
        Task Rollback();
        void CloseTransaction();
        void  Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(Guid id);
       IList<T> GetAll();
       IQueryable<T> Entity { get; }
       T GetByExpression(Expression<Func<T, bool>> expression);
        List<T> GetAllByExpression(Expression<Func<T, bool>> expression);

        void UpdateColumnForEntities<T>(IList<Guid> entityIds, string columnName, object newValue);
    }
}
