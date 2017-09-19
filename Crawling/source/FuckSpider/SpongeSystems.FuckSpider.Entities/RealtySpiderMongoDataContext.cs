
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SpongeSystems.Spider.Entities.Realty;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SpongeSystems.Spider.Entities
{
    public class RealtySpiderMongoDataContext<T> : IDisposable where T : IMongoEntity
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public RealtySpiderMongoDataContext()
        {
            //var credential = MongoCredential.CreateMongoCRCredential("test", "user1", "password1");
            //var settings = new MongoClientSettings
            //{
            //    Credentials = new[] { credential }
            //};

            _client = new MongoClient();
            _database = _client.GetDatabase("RealtySpider");
        }

        private IMongoCollection<T> GetCollection()
        {
            return _database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        //public IList<T> GetEntityCollection()
        //{
        //    return BsonSerializer.Deserialize<List<T>>(_database.GetCollection<BsonDocument>(typeof(T).Name.ToLower()).ToBsonDocument());
        //}

        private void Insert<T>(T record)
        {
            //BsonSerializer.Serialize<T>(

            ////await 
            //this.GetCollection().InsertOneAsync(document);
        }
        
        //public IMongoCollection<BsonDocument> Agencies
        //{
        //    get { return _database.GetCollection<BsonDocument>("Agency"); }
        //}

        //public IMongoCollection<BsonDocument> Categories
        //{
        //    get { return _database.GetCollection<BsonDocument>("Category"); }
        //}

        //public IMongoCollection<BsonDocument> Contacts
        //{
        //    get { return _database.GetCollection<BsonDocument>("Contact"); }
        //}

        //public IMongoCollection<BsonDocument> Purposes
        //{
        //    get { return _database.GetCollection<BsonDocument>("Purposes"); }
        //}


        //return BsonSerializer.Deserialize<List<Purpose>>(_database.GetCollection<BsonDocument>("Purposes").ToBsonDocument());


        //private readonly MongoQueryProvider provider;

        //public MongoQueryProvider Provider
        //{
        //    get { return provider; }
        //}

        //public static string DatabaseName { get; set; }

        //public RealtySpiderMongoDataContext()
        //{
        //    if ( string.IsNullOrEmpty( DatabaseName ) )
        //    {
        //        throw new InvalidOperationException( "You must set the static DatabaseName property." );
        //    }
        //    provider = new MongoQueryProvider( DatabaseName );
        //}

        //public IQueryable<Customer> Customers
        //{
        //    get { return new MongoQuery<Customer>( provider ); }
        //}

        //public IQueryable<Basic> Basics
        //{
        //    get { return new MongoQuery<Basic>( provider ); }
        //}

        //public MongoCollection<Basic> BasicCollection
        //{
        //    get { return provider.DB.GetCollection<Basic>(); }
        //}

        //public void Dispose()
        //{
        //    provider.Server.Dispose();
        //}

        //public void Add<T>(T entity) where T : class, new()
        //{
        //    provider.DB.GetCollection<T>().Insert( entity );
        //}

        //public void Delete<T>(T entity) where T : class, new()
        //{
        //    provider.DB.GetCollection<T>().Delete( entity );
        //}

        //public void Save<T>(T entity) where T : class, new()
        //{
        //    provider.DB.GetCollection<T>().Save( entity );
        //}

        //public void DropCollection<T>()
        //{
        //    provider.DB.DropCollection( typeof (T).Name );
        //}

        public void Dispose()
        {
            //_database.Client.c
        }
    }
}