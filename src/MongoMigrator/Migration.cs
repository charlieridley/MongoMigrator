using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoMigrator
{
    public abstract class Migration : IMigration
    {
        public abstract void Up(IMongoDatabase mongoDatabase);
        public int GetVersion()
        {
            return (this.GetType().GetCustomAttributes(typeof(MigrationAttribute), true).Single() as MigrationAttribute).Version;
        }
    }
}
