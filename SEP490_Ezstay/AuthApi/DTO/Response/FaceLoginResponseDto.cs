namespace AuthApi.DTO.Response
{
    public class FaceLoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public double? Confidence { get; set; }
    }

    public class RegisterFaceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public Guid? FaceId { get; set; }
    }

    public class VerifyFaceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public bool IsMatch { get; set; }
        public double? Confidence { get; set; }
    }

    public class FaceDataDto
    {
        public Guid Id { get; set; }
        public string? Label { get; set; }
        public string? ImageThumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetFacesResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<FaceDataDto> Faces { get; set; } = new List<FaceDataDto>();
    }

    public class UpdateFaceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }

    public class DeleteFaceResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
