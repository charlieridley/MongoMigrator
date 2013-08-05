using System;

namespace MongoMigrator
{
    public class DateTimeWrapper : IDateTimeWrapper
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}