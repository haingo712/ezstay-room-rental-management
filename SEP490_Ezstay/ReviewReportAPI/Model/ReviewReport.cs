using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ReviewReportAPI.Enum;
using ReviewReportAPI.Enum;

namespace ReviewReportAPI.Model;

public class ReviewReport
{
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid ReviewId { get; set; }
        public List<string> Images { get; set; }
        public string Reason { get; set; }
        public ReportStatus Status { get; set; }
        public string RejectReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ReviewedAt { get; set; } 
}

