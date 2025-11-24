using System;
using Microsoft.AspNetCore.Mvc;
using ChatAPI.DTO.Request;
using ChatAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace ChatAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ITokenService _tokenService;

    public ChatController(IChatService chatService, ITokenService tokenService)
    {
        _chatService = chatService;
        _tokenService = tokenService;
    }

    [Authorize(Roles = "User, Owner")]
    [HttpGet("messages/{chatRoomId}")]
    public async Task<IActionResult> GetMessagesByChatRoomId(Guid chatRoomId)
    {
        var receiverId= _tokenService.GetUserIdFromClaims(User);
       return Ok(await _chatService.GetByChatRoomId(chatRoomId, receiverId));
    }

    [Authorize(Roles = "Owner, User")]
    [HttpGet]
    public async Task<IActionResult> GetAllChatRoom()
    {
        var accountId= _tokenService.GetUserIdFromClaims(User);
      return  Ok(await _chatService.GetAllChatRoom(accountId));
    }
    
    [HttpPost("{ownerId}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateChatRoom(Guid ownerId)
    {
        var userId= _tokenService.GetUserIdFromClaims(User);
        return Ok(await _chatService.CreateChatRoom(ownerId, userId));
    }
    
    [HttpPost("message/{chatRoomId}")]
    [Authorize(Roles = "User, Owner")]
    public async Task<IActionResult> SendMessage(Guid chatRoomId, [FromForm] CreateChatMessage request)
    {
        var senderId= _tokenService.GetUserIdFromClaims(User);
       return  Ok(await _chatService.SendMessage(chatRoomId, senderId, request));
    }
    [HttpDelete("{chatMessageId}")]
    [Authorize(Roles = "User, Owner")]
    public async Task<IActionResult> RevokeMessage(Guid chatMessageId)
    {
        return Ok(await _chatService.Delete(chatMessageId));
    }
}
