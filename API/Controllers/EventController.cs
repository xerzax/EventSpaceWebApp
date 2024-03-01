using Application.DTOs;
using Application.DTOs.Event_DTO;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly IEventService _eventService;

		public EventController(IEventService eventService)
		{
			_eventService = eventService;
		}

		[HttpGet("GetAllEvents")]
		public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
		{
			return Ok(await _eventService.GetAllEventAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<EventDTO>> GetEventById(int id)
		{
			var evt = await _eventService.GetEventByIdAsync(id);
			if (evt == null)
			{
				return NotFound();
			}
			return Ok(evt);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEvent(int id, int qty)
		{
			await _eventService.UpdateEvent(id, qty);
			return NoContent();
		}

		[HttpPut("UpdateFund/{id}")]
		public async Task<IActionResult> UpdateFund(int id, int amount, int qty)
		{
			await _eventService.UpdateFundAsync(id, amount, qty);
			return NoContent();
		}

		[Authorize(Roles = "Organizer")]
		[HttpPost("CreateEvent")]
		public async Task<IActionResult> CreateEvent([FromBody]EventRequestDTO evt)
		{
			var createdEvent = await _eventService.CreateEventByAsync(evt);
			return Ok(createdEvent);
		}
	}
}
