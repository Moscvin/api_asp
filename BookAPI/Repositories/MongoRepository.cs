using System;
using System.Collections.Generic;
using BookAPI.Settings;
using Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookAPI.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoDocument
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDbSettings dbSettings)
        {
            if (string.IsNullOrEmpty(dbSettings.ConnectionString))
            {
                throw new ArgumentNullException(nameof(dbSettings.ConnectionString), "MongoDB ConnectionString cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(dbSettings.DatabaseName))
            {
                throw new ArgumentNullException(nameof(dbSettings.DatabaseName), "MongoDB DatabaseName cannot be null or empty.");
            }

            _db = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);
            string tableName = typeof(T).Name.ToLower();
            _collection = _db.GetCollection<T>(tableName);
        }



        public List<T> GetAllRecords()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }

      public T GetRecordById(Guid id)
{
    return _collection.Find(record => record.Id == id).FirstOrDefault();
}



        public T InsertRecord(T record)
        {
            _collection.InsertOne(record);
            return record;
        }

        public void UpsertRecord(T record)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, record.Id);
            _collection.ReplaceOne(filter, record, new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(record => record.Id, id);
            _collection.DeleteOne(filter);
        }
    }
}
