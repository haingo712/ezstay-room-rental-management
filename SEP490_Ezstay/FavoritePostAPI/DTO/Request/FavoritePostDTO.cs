namespace FavoritePostAPI.DTO.Request
{
    public class FavoritePostDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
