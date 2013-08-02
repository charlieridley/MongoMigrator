using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoMigrator
{
    public class VersionInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Version { get; set; }
    }
}
