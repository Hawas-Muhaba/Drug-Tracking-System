using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrugSystem.Models{


    public class Pharmacy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Nullable to avoid warnings

        public required string Name { get; set; }
        public required Location Location { get; set; }
    }

}