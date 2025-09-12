using BoardingHouseAPI.DTO.Request;
using BoardingHouseAPI.Service.Interface;
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
        public BoardingHousesController(IBoardingHouseService boardingHouseService, ITokenService tokenService)
        {
            _boardingHouseService = boardingHouseService;
            _tokenService = tokenService;
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
        public async Task<IActionResult> PutBoardingHouse(Guid id, UpdateBoardingHouseDTO dto)
        {
            try
            {
                var response = await _boardingHouseService.UpdateAsync(id, dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }
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
        public async Task<ActionResult<BoardingHouseDTO>> PostBoardingHouse(CreateBoardingHouseDTO dto)
        {
            var ownerId = _tokenService.GetUserIdFromClaims(User);            
            try
            {
                var response = await _boardingHouseService.CreateAsync(ownerId, dto);
                if (!response.IsSuccess)
                {
                    return BadRequest(new { message = response.Message });
                }
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
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
