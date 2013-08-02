using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MongoMigrator
{
    public class MigrationReader : IMigrationReader
    {
        public IEnumerable<IMigration> GetFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            return assembly.GetTypes()
                           .Where(x => x.GetCustomAttributes<MigrationAttribute>().Any())
                           .Select(x => Activator.CreateInstance(x) as IMigration);
        }
    }
}