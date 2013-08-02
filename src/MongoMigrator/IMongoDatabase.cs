namespace MongoMigrator
{
    public interface IMongoDatabase
    {
        IModelCollection<T> GetCollection<T>();
    }
}