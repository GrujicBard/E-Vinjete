using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VignetteAuth.Models
{
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Registration { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Country { get; set; }

    }
}
