using MongoDB.Driver;
using SpongeSystems.Spider.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Services.Contracts
{
    public interface IBaseService<T> where T : IMongoEntity
    {
        //void Insert(T entity);
        //void Delete(string id);
        //void Update(T entity);
        //Task<IAsyncCursor<T>> GetById(string id);        
        //IList<T> GetAll();

        Task<T> Get(string id);
        Task Insert(T record);
        Task Delete(string id);
        Task Delete(T record);
        Task Update(T record);
        Task<IEnumerable<T>> FindAll();
    }
}
