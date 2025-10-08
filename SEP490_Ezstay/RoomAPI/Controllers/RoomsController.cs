using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using RoomAPI.DTO.Request;
using RoomAPI.DTO.Response;
using RoomAPI.Model;
using RoomAPI.Service;
using RoomAPI.Service.Interface;

namespace RoomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        
        [HttpPut("{id}/RoomStatus/{roomStatus}")]
        // [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateStatus(Guid id, string roomStatus)
        {
            try
            {
                var result = await _roomService.UpdateStatusAsync(id, roomStatus);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet("{id}/WithAmenities")]
        public async Task<ActionResult<RoomWithAmenitiesDto>> GetRoomWithAmenities(Guid id)
        {
            try
            {
                var result = await _roomService.GetRoomWithAmenitiesAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }
        
        
        [HttpGet]
        [EnableQuery]
        public IQueryable<RoomDto> GetRooms()
        {
            return _roomService.GetAllQueryable();
        }
        
        [HttpGet("ByHouseId/{houseId}")]
        [EnableQuery]        
        public IQueryable<RoomDto> GetRoomsByHouseId(Guid houseId)
        {
            return _roomService.GetAllByHouseId(houseId);
        }
        [HttpGet("ByHouseId/{houseId}/Status")]
        [EnableQuery]        
        public IQueryable<RoomDto> GetRoomsStatusByHouseId(Guid houseId)
        {
            return _roomService.GetAllStatusActiveByHouseId(houseId);
        }
        // [HttpGet("ByHouseId/{houseId}")]
        // [EnableQuery]        
        // public IQueryable<RoomDto> GetRoomsStatusActiveByHouseId(Guid houseId)
        // {
        //     return _roomService.GetAllByHouseId(houseId);
        // }
    
        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoomById(Guid id)
        {
            try
            {
                var room = await _roomService.GetById(id);
                return room;
            }
            catch (KeyNotFoundException e)
            {
             return NotFound(new { message = e.Message });
            }
          
        }
        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
    
        public async Task<IActionResult> PutRoom(Guid id,[FromForm] UpdateRoomDto request) {
        try
        {
          var result =  await _roomService.Update(id, request);
          if (!result.IsSuccess)
          {
              return BadRequest(new { message = result.Message });
          }
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }
        
    [HttpPost("House/{houseId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> PostRoom(Guid houseId,[FromForm] CreateRoomDto request)
    {
        try
        {
            //  var ownerId = _tokenService.GetUserIdFromClaims(User);
            var createRoom =   await  _roomService.Add( houseId,request);
            if (!createRoom.IsSuccess)
                return BadRequest(new { message = createRoom.Message });
         
            return CreatedAtAction("GetRoomById", new { id = createRoom.Data.HouseId }, createRoom);

        }catch (Exception e) {
            return Conflict(new { message = e.Message });
        }
    }
    //     // DELETE: api/Rooms/5
    // [Authorize(Roles = "Owner")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        try
        {    
          var room=  await  _roomService.Delete(id);
            if (!room.IsSuccess)
                return BadRequest(new { message = room.Message });
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    }
}
