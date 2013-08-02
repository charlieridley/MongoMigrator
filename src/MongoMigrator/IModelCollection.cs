using System.Linq;

namespace MongoMigrator
{
    public interface IModelCollection<T>
    {
        IQueryable<T> Get();
        void Save(T document);
        void RemoveAll();
    }
}