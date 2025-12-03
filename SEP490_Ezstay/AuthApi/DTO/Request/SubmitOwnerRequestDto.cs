namespace AuthApi.DTO.Request
{
    public class SubmitOwnerRequestDto
    {
        public string Reason { get; set; } = null!;

        public Guid AccountId { get; set; }

        public IFormFile Imageasset { get; set; } = null!;
    }
}
