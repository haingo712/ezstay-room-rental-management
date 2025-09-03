using Microsoft.AspNetCore.Mvc;

namespace RentalRequestAPI.Controllers;

public class RentalRequestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}