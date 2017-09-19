using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace SpongeSolutions.ServicoDireto.Services.InfraStructure
{
    [System.ServiceModel.ServiceContract]
    public interface IBaseRepository<T, C>
    {
        void CreateDatabase();

        void CreateAndDropDatabase();

        void Insert(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);

        T GetById(C id);

        void Update(T entity);

        void Update(Expression<Func<T, bool>> predicate);

        void Update(T oldEntity, T newEntity);

        void Attach(T entidade);

        /* Desanexa a entidade especificada */
        void Detach(T entidade);
            
        void SaveChanges();

        IQueryable<T> Fetch();

        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Func<T, bool> predicate);

        T Single(Func<T, bool> predicate);

        T First(Func<T, bool> predicate);

        T FirstOrDefault(Func<T, bool> predicate);

        T Last(Func<T, bool> predicate);
            
        T LastOrDefault(Func<T, bool> predicate);

        void Dispose();
    }

    public interface IBaseRepository<T> : IBaseRepository<T, int>
    { }
}
