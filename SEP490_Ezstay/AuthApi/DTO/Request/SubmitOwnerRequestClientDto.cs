namespace AuthApi.DTO.Request
{
    public class SubmitOwnerRequestClientDto
    {
        public string Reason { get; set; } = null!;

        public IFormFile Imageasset { get; set; }

    }
}
