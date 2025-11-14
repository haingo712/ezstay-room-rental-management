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
using RoomAPI.Model;
using RoomAPI.Service;
using RoomAPI.Service.Interface;
using Shared.DTOs.RoomAmenities.Responses;
using Shared.DTOs.Rooms.Responses;
using Shared.Enums;

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
        public async Task<IActionResult> UpdateStatus(Guid id, RoomStatus roomStatus)
        {
            try
            {
                var result = await _roomService.UpdateStatus(id, roomStatus);
                if (!result.IsSuccess)
                    return BadRequest(new { message = result.Message });
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        // [HttpGet("{id}/WithAmenities")]
        // public async Task<ActionResult<RoomWithAmenitiesResponse>> GetRoomWithAmenities(Guid id)
        // {
        //     try
        //     {
        //         var result = await _roomService.GetRoomWithAmenitiesAsync(id);
        //         return Ok(result);
        //     }
        //     catch (KeyNotFoundException e)
        //     {
        //         return NotFound(new { message = e.Message });
        //     }
        // }
        
        [HttpGet("ByHouseId/{houseId}")]
        [EnableQuery]        
        public IQueryable<RoomResponse> GetRoomsByHouseId(Guid houseId)
        {
            return _roomService.GetAllByHouseId(houseId);
        }
        [HttpGet("ByHouseId/{houseId}/Status")]
        [EnableQuery]           
        public IQueryable<RoomResponse> GetRoomsStatusByHouseId(Guid houseId)
        {
            return _roomService.GetAllStatusActiveByHouseId(houseId);
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var room = await _roomService.GetById(id);
                return Ok(room);
            }
            catch (KeyNotFoundException e)
            {
             return NotFound(new { message = e.Message });
            }
          
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Put(Guid id,[FromForm] UpdateRoom request) {
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
    public async Task<IActionResult> Post(Guid houseId,[FromForm] CreateRoom request)
    {
        try
        {
            var createRoom =   await  _roomService.Add(houseId, request);
            if (!createRoom.IsSuccess)
                return BadRequest(new { message = createRoom.Message });
            return CreatedAtAction("GetById", new { id = createRoom.Data.HouseId }, createRoom);
        }catch (Exception e) {
            return Conflict(new { message = e.Message });
        }
    }
    [Authorize(Roles = "Owner")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
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
