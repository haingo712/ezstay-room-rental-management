using System.ComponentModel.DataAnnotations;

namespace ChatAPI.DTO.Request;

public class CreateChatRoom
{
  //  public Guid PostId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid TenantId { get; set; }
}