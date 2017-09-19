using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpongeSystems.Spider.Entities;
using SpongeSystems.Spider.Services.Contracts;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SpongeSystems.Spider.Services.Implementation
{
    public abstract class BaseService<T> : IBaseService<T> where T : IMongoEntity
    {
        private IMongoDatabase _db = null;
        public IMongoCollection<T> _collection = null;

        public IMongoCollection<T> Collection { get { return _collection; } }

        protected BaseService(string databaseName = null)
        {
            if (string.IsNullOrEmpty(databaseName))
                databaseName = "RealtySpider";

            const string connectionString = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            _db = mongoClient.GetDatabase(databaseName);
            _collection = this._db.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> Get(string id)
        {
            return await _collection.Find(x => x.Id == new ObjectId(id)).SingleAsync();
        }

        public async Task Insert(T record)
        {
            await _collection.InsertOneAsync(record);
        }

        public async Task Delete(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == new ObjectId(id));
        }

        public async Task Delete(T record)
        {
            await _collection.DeleteOneAsync(x => x.Id == record.Id);
        }

        public async Task Update(T record)
        {
            await _collection.ReplaceOneAsync(x => x.Id == record.Id, record);
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            var Ts = await _collection.Find("{}").ToListAsync();
            return Ts;
        }

        

        ////public async void Insert(T entity)
        ////{
        ////    await _collection.InsertOneAsync(entity);
        ////}

        //public void Insert(T entity)
        //{
        //    _collection.InsertOneAsync(entity).Wait();
        //}

        //public void Delete(string id)
        //{
        //    _collection.DeleteOneAsync(p => p.Id == new ObjectId(id)).Wait();
        //}

        //public void Update(T entity)
        //{
        //    _collection.ReplaceOneAsync(p => p.Id == entity.Id, entity).Wait();
        //}

        //public T GetById(string id)
        //{
        //    return _collection.Find(p => p.Id == new ObjectId(id)).SingleAsync().Wait();
        //}

        //public IList<T> GetAll()
        //{
        //    return _collection.Find("{}").ToListAsync().Wait();
        //}
    }
}
