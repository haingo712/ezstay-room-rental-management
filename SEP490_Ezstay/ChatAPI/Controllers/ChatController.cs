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
        => Ok(await _chatService.GetByChatRoomId(chatRoomId));
    
    
    // [HttpGet("chat-room/{chatRoomId}")]
    // public async Task<IActionResult> GetRoomWithPost(Guid chatRoomId)
    // {
    //     var response = await _chatService.GetRoomWithPost(chatRoomId);
    //     if (!response.IsSuccess)
    //         return NotFound(response); 
    //     return Ok(response); 
    // }
  

    [Authorize(Roles = "Owner, User")]
    [HttpGet]
    public async Task<IActionResult> GetAllChatRoom()
    {
        var accountId= _tokenService.GetUserIdFromClaims(User);
      return  Ok(await _chatService.GetAllChatRoom(accountId));
    }

   
    // lam 
    [HttpPost]
    [Authorize(Roles = "User, Owner")]
    public async Task<IActionResult> CreateChatRoom([FromQuery] Guid postId)
    {
        var userId= _tokenService.GetUserIdFromClaims(User);
        return Ok(await _chatService.Add(postId, userId));
    }
    
    //lam r
    [HttpPost("message")]
    [Authorize(Roles = "User, Owner")]
    public async Task<IActionResult> SendMessage([FromQuery] Guid chatRoomId, [FromBody] CreateChatMessage request)
    {
        var senderId= _tokenService.GetUserIdFromClaims(User);
       return  Ok(await _chatService.SendMessage(chatRoomId, senderId, request));
    }
}
