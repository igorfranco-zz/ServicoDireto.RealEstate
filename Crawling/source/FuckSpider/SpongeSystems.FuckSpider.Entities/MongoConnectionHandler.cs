using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.Spider.Entities
{
    public abstract class MongoConnectionHandler<T> where T : IMongoEntity
    {
        public IMongoCollection<T> MongoCollection { get; private set; }
        private IMongoDatabase _db = null;
        private const string databaseName = "realtyspider";

        public MongoConnectionHandler()
        {
            const string connectionString = "mongodb://localhost";

            //// Get a thread-safe client object by using a connection string
            var mongoClient = new MongoClient(connectionString);

            //// Get a reference to a server object from the Mongo client object
            //var mongoServer = mongoClient.GetServer();

            //// Get a reference to the "retrogamesweb" database object 
            //// from the Mongo server object

            _db = mongoClient.GetDatabase(databaseName);
        }

        private IMongoCollection<T> GetCollection()
        {
            return this._db.GetCollection<T>(typeof(T).Name.ToLower());
        }
    }    
}
