namespace MongoMigrator
{
    public class MongoDatabase : IMongoDatabase
    {
        private readonly MongoDB.Driver.MongoDatabase database;

        public MongoDatabase(MongoDB.Driver.MongoDatabase database)
        {
            this.database = database;            
        }

        public IModelCollection<T> GetCollection<T>()
        {
            var mongoCollection = database.GetCollection<T>(typeof (T).Name);
            return new ModelCollection<T>(mongoCollection);
        }
    }
}