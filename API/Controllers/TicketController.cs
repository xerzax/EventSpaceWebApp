using Application.DTOs.Ticket;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TicketController : ControllerBase
	{
		private readonly ITicketService _ticketService;

		public TicketController(ITicketService ticketService)
		{
			_ticketService = ticketService;
		}

		[Authorize]
		[HttpPost("buy-tickets")]
		public async Task<IActionResult> BuyTicketAsync(TicketRequestDTO ticketRequest)
		{
			var result = await _ticketService.BuyTicketsAsync(ticketRequest);
			return Ok(result);
		}
	}
}
