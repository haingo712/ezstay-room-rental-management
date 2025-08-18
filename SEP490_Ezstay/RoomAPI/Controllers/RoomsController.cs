using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using RoomAPI.Data;
using RoomAPI.DTO.Request;
using RoomAPI.Model;
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

        // GET: api/Rooms
        [HttpGet]
        [EnableQuery]
        public IQueryable<RoomDto> GetRooms()
        {
            return _roomService.GetAll();
        }
        [HttpGet("ByHouseId/{houseId}")]
        [EnableQuery]
      //  [Authorize(Roles = "Owner")]
        public IQueryable<RoomDto> GetRoomsByHouseId(Guid houseId)
        {
            return _roomService.GetAllByHouseId(houseId);
        }
        [HttpGet("ByHouseLocationId/{houseLocationId}")]
        [EnableQuery]
     //   [Authorize(Roles = "Owner")]
        public IQueryable<RoomDto> GetRoomsByHouseLocationId(Guid houseLocationId)
        {
            return _roomService.GetAllByHouseLocationId(houseLocationId);
        }
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
   // [Authorize(Roles = "Owner")]
    public async Task<IActionResult> PutRoom(Guid id, UpdateRoomDto request)
    {
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
    //     // POST: api/Rooms
    [HttpPost]
   //  [Authorize(Roles = "Owner")]
    public async Task<IActionResult> PostRoom(CreateRoomDto request)
    {
        try
        {
         var createRoom =   await  _roomService.Add(request);
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
            await  _roomService.Delete(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
     
    }
    }
}
