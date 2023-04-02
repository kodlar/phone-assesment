using MongoDB.Bson.Serialization.Attributes;

namespace PhoneBook.Domain.Entities
{
    public class PhoneBook
    { 
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        public string Company { get; set; }
        public ContactInfo? Contact { get; set; }
        public bool IsDeleted { get; set; }

    }
}
