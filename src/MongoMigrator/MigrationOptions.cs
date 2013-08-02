using System.Reflection;

namespace MongoMigrator
{
    public class MigrationOptions
    {
        public string ConnectionString { get; set; }

        public Assembly Assembly { get; set; }
    }
}