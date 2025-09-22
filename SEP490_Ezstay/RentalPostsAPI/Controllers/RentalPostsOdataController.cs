using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.Service.Interface;

namespace RentalPostsAPI.Controllers;

public class RentalPostsOdataController : ODataController
{
   
    private readonly IRentalPostService _service;

    public RentalPostsOdataController(IRentalPostService service)
    {
        _service = service;
    }

    [HttpGet("Odata/RentalPostsOdata")]
    [EnableQuery]
    public IQueryable<RentalpostDTO> GetRentalPostsOdata()
    {
        return  _service.GetAllAsQueryable();
    }
}