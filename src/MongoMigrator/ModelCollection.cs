using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoMigrator
{
    public class ModelCollection<T> : IModelCollection<T>
    {
        private readonly MongoCollection<T> mongoCollection;

        public ModelCollection(MongoCollection<T> mongoCollection)
        {
            this.mongoCollection = mongoCollection;
        }

        public IQueryable<T> Get()
        {
            return mongoCollection.AsQueryable();
        }

        public void Save(T document)
        {
            mongoCollection.Save(document);
        }

        public void RemoveAll()
        {
            mongoCollection.RemoveAll();
        }
    }
}