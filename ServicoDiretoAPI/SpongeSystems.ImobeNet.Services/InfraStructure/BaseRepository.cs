using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data;
using System.Transactions;
using System.Web;
using System.Data.Entity.Validation;

namespace SpongeSolutions.ServicoDireto.Services.InfraStructure
{
    //public abstract class BaseRepository<T, C> : IBaseRepository<T> 
    //    where T : class
    //    where C : DbContext
    public class BaseRepository<T, C> : IBaseRepository<T, C>
        where T : class
    {
        #region [Attributes]

        protected DbSet<T> _dbSet;
        protected DbContext _dataContext;

        #endregion

        public DbSet<T> dbSet { get { return this._dbSet; } }

        public BaseRepository(DbContext dataContext)
        {
            //this._dataContext = ((C)Activator.CreateInstance(typeof(C))); ;
            _dataContext = dataContext;
            this._dbSet = dataContext.Set<T>();
        }

        virtual public void CreateDatabase()
        {
            this._dataContext.Database.CreateIfNotExists();
        }

        virtual public void CreateAndDropDatabase()
        {
            if (this._dataContext.Database.Exists())
            {
                this._dataContext.Database.Delete();
            }

            this._dataContext.Database.Create();
        }

        virtual public void Insert(T entity)
        {
            this._dbSet.Add(entity);
        }

        virtual public void Delete(T entity)
        {
            this._dbSet.Remove(entity);
        }

        virtual public void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> records = from x in this._dbSet.Where<T>(predicate) select x;

            foreach (T record in records)
            {
                this._dbSet.Remove(record);
            }
        }

        virtual public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return this._dbSet.Where(predicate);
        }

        virtual public T GetById(C id)
        {
            return this._dbSet.Find(id);
        }

        virtual public void Update(T oldEntity, T newEntity)
        {
            this._dataContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        virtual public void Update(T entity)
        {
            this._dataContext.Entry(entity).State = EntityState.Modified;
        }

        virtual public void Update(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> records = from x in this._dbSet.Where<T>(predicate) select x;

            foreach (T record in records)
            {
                this._dataContext.Entry(record).State = EntityState.Modified;
            }
        }

        virtual public void Attach(T entidade)
        {
            this._dbSet.Attach(entidade);
        }

        virtual public void Detach(T entidade)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._dataContext).ObjectContext.Detach(entidade);
        }

        virtual public void SaveChanges(bool useTransaction = true)
        {
            if (useTransaction)
                this.SaveChanges();
            else
                this._dataContext.SaveChanges();
        }

        virtual public void SaveChanges()
        {
            Boolean savedChanges = false;

            using (TransactionScope scopeOfTransaction = new TransactionScope())
            {
                try
                {
                    this._dataContext.SaveChanges();
                    savedChanges = true;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    savedChanges = false;
                    throw;
                }
                catch
                {
                    //this._dataContext.SaveChanges();
                    savedChanges = false;
                }
                finally
                {
                    if (savedChanges)
                    {
                        scopeOfTransaction.Complete();
                    }
                }
            }
        }

        virtual public IQueryable<T> Fetch()
        {
            try
            {
                return this._dbSet.AsQueryable<T>();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public IEnumerable<T> GetAll()
        {
            try
            {
                return this._dbSet.AsEnumerable<T>();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }


        virtual public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.Where<T>(predicate);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public long Count(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.Count(predicate);
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
        }

        virtual public T Single(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.Single<T>(predicate);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public T First(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.First<T>(predicate);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public T FirstOrDefault(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.FirstOrDefault<T>(predicate);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public T Last(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.Last<T>(predicate);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public T LastOrDefault(Func<T, bool> predicate)
        {
            try
            {
                return this._dbSet.LastOrDefault<T>(predicate);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        virtual public void Dispose()
        {
            this._dataContext.Dispose();
        }
    }

    public class BaseRepository<T> : BaseRepository<T, int>
       where T : class
    {
        public BaseRepository(DbContext dataContext) : base(dataContext)
        {

        }
    }
}
