// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.OData.Query;
// using Microsoft.EntityFrameworkCore;
// using TenantAPI.Data;
// using TenantAPI.DTO.Request;
// using TenantAPI.Models;
// using TenantAPI.Service.Interface;
//
// namespace TenantAPI.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//    
//     public class TenantController : ControllerBase
//     {
//         private readonly ITenantService _tenantService;
//
//         public TenantController(ITenantService tenantService)
//         {
//             _tenantService = tenantService;
//         }
//         
//         [Authorize(Roles = "Admin")]
//         [HttpGet]
//         [EnableQuery]
//         public  IQueryable<TenantDto> GetTenants()
//         {
//             return  _tenantService.GetAll();
//         }
//         
//         [Authorize(Roles = "Owner")]
//         [HttpGet("ByUserId/{userId}")]
//         [EnableQuery]
//         public  IQueryable<TenantDto> GetTenantsByUserId(Guid userId )
//         {
//             return  _tenantService.GetAllByUserId(userId);
//         }
//         
//         [Authorize(Roles = "Owner")]
//         [HttpGet("ByRoomId/{roomId}")]
//         [EnableQuery]
//         public  IQueryable<TenantDto> GetTenantsByRoomId(int roomId )
//         {
//         
//             return  _tenantService.GetAllByRoomId(roomId);
//         }
//
//         // GET: api/Tenant/5
//       //  [Authorize(Roles = "Owner")]
//         [HttpGet("{id}")]
//         public async Task<ActionResult<TenantDto>> GetTenant(int id)
//         {
//             try
//             {
//                 var tenant = await _tenantService.GetByIdAsync(id);
//                 return Ok(tenant);
//             }
//             catch (KeyNotFoundException e)
//             {
//                 return NotFound(new { message = e.Message });
//             }
//         }
//         [Authorize(Roles = "Owner")]
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutTenant(int id, UpdateTenantDto request)
//         {
//             try
//             {
//                var result = await _tenantService.UpdateAsync(id, request);
//                 if (!result.IsSuccess  )
//                 {
//                     return BadRequest(new { message = result.Message });
//                 }
//                 return NoContent();
//             }
//             catch (KeyNotFoundException e)
//             {
//                 return NotFound(new { message = e.Message });
//             }
//         }
//         
//         [Authorize(Roles = "Owner")]
//         [HttpPost]
//         public async Task<ActionResult<TenantDto>> PostTenant(CreateTenantDto request)
//         {
//             try
//             {
//               
//              var createTenant=  await _tenantService.AddAsync( request);
//            
//              if (!createTenant.IsSuccess ) // || createTenant.Data == null)
//              {
//                  return BadRequest(new { message = createTenant.Message });
//              }
//                 return CreatedAtAction("GetTenant", new { id = createTenant.Data.TenantId }, createTenant);
//             }
//             catch (KeyNotFoundException e)
//             {
//                 return NotFound(new { message = e.Message });
//             }
//
//         }
//
//         // // DELETE: api/Tenant/5
//         // [HttpDelete("{id}")]
//         // public async Task<IActionResult> DeleteTenant(int id)
//         // {
//         //     try
//         //     {
//         //         await _tenantService.DeleteAsync(id);
//         //         return NoContent();
//         //     }
//         //     catch (KeyNotFoundException e)
//         //     {
//         //         return NotFound(new { message = e.Message });
//         //     }
//         // }
//     }
// }
