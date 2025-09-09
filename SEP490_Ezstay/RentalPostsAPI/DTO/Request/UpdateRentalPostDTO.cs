namespace RentalPostsAPI.DTO.Request
{
    public class UpdateRentalPostDTO
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string ContactPhone { get; set; } = null!;
    }
}
