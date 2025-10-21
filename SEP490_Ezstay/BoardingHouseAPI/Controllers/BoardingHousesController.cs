using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.DTO.Response;
using BoardingHouseAPI.Enum;
using BoardingHouseAPI.Service.Interface;
using EasyNetQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace BoardingHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BoardingHousesController : ControllerBase
    {
        private readonly IBoardingHouseService _boardingHouseService;
        private readonly ITokenService _tokenService;
        private readonly IBus _bus;
        public BoardingHousesController(IBoardingHouseService boardingHouseService, ITokenService tokenService, IBus bus)
        {
            _boardingHouseService = boardingHouseService;
            _tokenService = tokenService;
            _bus = bus;
        }
       
        // GET: api/BoardingHouses
        [HttpGet]
        [EnableQuery]        
        public IQueryable<BoardingHouseDTO> GetBoardingHouses()
        {
            return _boardingHouseService.GetAll();            
        }        

        // GET: api/BoardingHouses/owner
        [HttpGet("owner")]
        [EnableQuery]
        [Authorize(Roles = "Owner")]
        public IQueryable<BoardingHouseDTO> GetBoardingHousesByOwner()
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);
            return _boardingHouseService.GetByOwnerId(ownerId);
        }

        // GET: api/BoardingHouses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardingHouseDTO>> GetBoardingHouse(Guid id)
        {
            try 
            { 
                var house = await _boardingHouseService.GetByIdAsync(id);
                return Ok(house);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/BoardingHouses/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> PutBoardingHouse(Guid id, [FromForm] UpdateBoardingHouseDTO dto)
        {
            try
            {
                var response = await _boardingHouseService.UpdateAsync(id, dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }

                var notification = new
                {
                    EventType = "BoardingHouseChanged",
                    Data = new {Id = id, UpdatedFields = dto }
                };
                var jsonMessage = System.Text.Json.JsonSerializer.Serialize(notification);
                //await _bus.PubSub.PublishAsync(jsonMessage);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/BoardingHouses
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<BoardingHouseDTO>> PostBoardingHouse([FromForm] CreateBoardingHouseDTO dto)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);            
            try
            {
                var response = await _boardingHouseService.CreateAsync(ownerId, dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }               

                var notification = new
                {
                    EventType = "BoardingHouseChanged",
                    Data = dto
                };
                var jsonMessage = System.Text.Json.JsonSerializer.Serialize(notification);
                //await _bus.PubSub.PublishAsync(jsonMessage);

                return CreatedAtAction(nameof(GetBoardingHouse), new { id = response.Data.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/BoardingHouses/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteBoardingHouse(Guid id)
        {
            try
            {
                var response = await _boardingHouseService.DeleteAsync(id);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }
                var notification = new
                {
                    EventType = "BoardingHouseChanged",
                    Data = new { Id = id }
                };
                var jsonMessage = System.Text.Json.JsonSerializer.Serialize(notification);
                //await _bus.PubSub.PublishAsync(jsonMessage);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("rank")]
        public async Task<ActionResult<List<BoardingHouseRankResponse>>> GetRankedBoardingHouses(
            [FromQuery] RankType type,
            [FromQuery] string order = "desc",
            [FromQuery] int limit = 10)
        {
            try
            {
                var result = await _boardingHouseService.GetRankedBoardingHousesAsync(type, order, limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/sentiment-feedback")]
        public async Task<IActionResult> GetSentimentFeedback(Guid id)
        {
            var response = await _boardingHouseService.GetSentimentSummaryAsync(id);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}/rating-feedback")]
        public async Task<IActionResult> GetRatingFeedback(Guid id)
        {
            var response = await _boardingHouseService.GetRatingSummaryAsync(id);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }


    }
}
