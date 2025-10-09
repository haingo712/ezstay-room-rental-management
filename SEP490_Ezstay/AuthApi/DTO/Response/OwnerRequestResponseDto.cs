using AuthApi.Enums;

namespace AuthApi.DTO.Response
{
    public class OwnerRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime SubmittedAt { get; set; }
        public RequestStatusEnum Status { get; set; }

        public string? RejectionReason { get; set; }
    }
}
