using Shared.DTOs.Auths.Responses;
using Shared.Enums;
namespace Shared.DTOs.Auths.Responses
{
    public class OwnerRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime SubmittedAt { get; set; }
        public RequestStatusEnum Status { get; set; }
        public string Imageasset { get; set;}
        public string? RejectionReason { get; set; }
    }
}
