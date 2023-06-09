﻿using MongoDB.Bson.Serialization.Attributes;
using PhoneBook.Domain.Enums;

namespace PhoneBook.Domain.Entities
{
    public  class PhoneBookReports
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }        
        public string TraceReportId { get; set; }
        public LocationReportItem? Report { get; set; }
        public ReportEnum Status { get; set; }
        public DateTime  CreatedAt  { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
