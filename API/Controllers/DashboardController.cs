using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dasboardService;

        public DashboardController(IDashboardService dasboardService)
        {
            _dasboardService = dasboardService;
        }

        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _dasboardService.GetAllUsers("User");

            return Ok(result);
        }

        [HttpGet("get-all-organizer")]
        public async Task<IActionResult> GetAllOrganizer()
        {
            var result = await _dasboardService.GetAllUsers("Organizer");

            return Ok(result);
        }

        [HttpGet("get-all-event-details")]
        public async Task<IActionResult> GetEventDetails()
        {
            var result = await _dasboardService.GetEventDetails();

            return Ok(result);
        }
    }
}
