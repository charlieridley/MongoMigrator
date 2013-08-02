using System.Collections.Generic;
using System.Reflection;

namespace MongoMigrator
{
    public interface IMigrationReader
    {
        IEnumerable<IMigration> GetFromAssembly(Assembly migrations);
    }
}