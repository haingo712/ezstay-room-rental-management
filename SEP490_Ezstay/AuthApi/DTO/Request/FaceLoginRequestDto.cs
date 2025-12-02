namespace AuthApi.DTO.Request
{
    public class FaceLoginRequestDto
    {
        public string FaceImage { get; set; } = null!;
    }
    public class RegisterFaceRequestDto
    {
        public string FaceImage { get; set; } = null!;
        public string? Label { get; set; }
    }
    public class VerifyFaceRequestDto
    {
        public string FaceImage { get; set; } = null!;
    }
    public class UpdateFaceRequestDto
    {
        public Guid FaceId { get; set; }
        public string? FaceImage { get; set; }
        public string? Label { get; set; }
    }
    public class DeleteFaceRequestDto
    {
        public Guid FaceId { get; set; }
    }
}
