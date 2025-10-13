using MapOfCafesNearUniversity.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MapOfCafesNearUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILeafletService _leafletService;
        public HomeController(ILeafletService leafletService)
        {
            _leafletService = leafletService;
        }

        public async Task<IActionResult> Index()
        {
            var cafes = await _leafletService.GetCafes();
            var cafesJson = JsonSerializer.Serialize(cafes, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            ViewData["CafesJson"] = cafesJson;

            return View();
        }
    }
}
