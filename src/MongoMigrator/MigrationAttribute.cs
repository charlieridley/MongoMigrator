﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoMigrator
{
    public class MigrationAttribute : Attribute
    {
        public long Version { get; set; }

        public MigrationAttribute(long version)
        {
            Version = version;
        }
    }
}
