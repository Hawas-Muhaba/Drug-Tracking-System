using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace DrugSystem.Models
{

    public class Drug
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Name { get; set; }
        public required string PharmacyId { get; set; }
        public required double Price { get; set; }
        public required int Quantity { get; set; }
    }

}

