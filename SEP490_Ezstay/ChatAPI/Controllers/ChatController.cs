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
    [HttpGet("chat-room/{chatRoomId}")]
    public async Task<IActionResult> GetRoomWithPost(Guid chatRoomId)
    {
        var response = await _chatService.GetRoomWithPost(chatRoomId);
        if (!response.IsSuccess)
            return NotFound(response); 
        return Ok(response); 
    }
   
    [HttpPost]
    [Authorize(Roles = "User, Owner")]
    public async Task<IActionResult> CreateChatRoom([FromQuery] Guid postId)
    {
        var userId= _tokenService.GetUserIdFromClaims(User);
        return Ok(await _chatService.Add(postId, userId));
    }

    [Authorize(Roles = "Owner, User")]
    [HttpGet]
    public async Task<IActionResult> GetAllChatRoomByOwner()
    {
        var ownerId= _tokenService.GetUserIdFromClaims(User);
      return  Ok(await _chatService.GetAllChatRoomByOwner(ownerId));
    }

    // [Authorize(Roles = "User")]
    // [HttpGet("chat-room")]
    // public async Task<IActionResult> GetUserRooms()
    // {
    //     var userId= _tokenService.GetUserIdFromClaims(User);
    //   return   Ok(await _chatService.GetChatRoomsByTenant(userId));
    // }
    // lam
    [Authorize(Roles = "User, Owner")]
    [HttpGet("messages/{chatRoomId}")]
    public async Task<IActionResult> GetMessages(Guid chatRoomId)
        => Ok(await _chatService.GetMessages(chatRoomId));
    //lam
    [HttpPost("message")]
    [Authorize(Roles = "User, Owner")]
    public async Task<IActionResult> SendMessage([FromQuery] Guid chatRoomId, [FromBody] CreateChatMessage request)
    {
        var userId= _tokenService.GetUserIdFromClaims(User);
       return  Ok(await _chatService.SendMessage(chatRoomId, userId, request));
    }
}
