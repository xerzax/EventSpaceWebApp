using Application.DTOs;
using Application.DTOs.Ticket;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Domain.Entity.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TicketController : ControllerBase
	{
		private readonly ITicketService _ticketService;
		private readonly IEventService _eventService;
		private readonly IUserIdentityService _idenityService;

		private readonly ITierService _tierService;



		public TicketController(ITicketService ticketService, IEventService eventService, IUserIdentityService idenityService, ITierService tierService)
		{
			_ticketService = ticketService;
			_eventService = eventService;
			_idenityService = idenityService;
			_tierService = tierService;
		}

		[Authorize]
		[HttpPost("buy-tickets")]
		public async Task<IActionResult> BuyTicketAsync(TicketRequestDTO ticketRequest)
		{
			var result = await _ticketService.BuyTicketsAsync(ticketRequest);
			return Ok(result);
		}


        [HttpPost("Confirm/{ticketCode}")]
        public async Task<IActionResult> Confirm(string ticketCode)
		{
			var res = await _ticketService.ConfirmTicket(ticketCode);
			return Ok(res);


		}

        [HttpPost("Payment")]
        public async Task<IActionResult> Payment(PaymentRequestDTO model)
        {
            EventDTO eventDetail = new EventDTO();

			TierDTO tierDetail = new TierDTO(); 
            int price = 0;
			string name = "";
			eventDetail = await _eventService.GetEventByIdAsync(model.EventId);



                if (!model.TierId.HasValue)

                {
                    price = Convert.ToInt32(eventDetail.StartingPrice) * model.Qty;
                name = eventDetail.Name ?? "";

            }

			if (model.TierId.HasValue)
			{
				tierDetail = await _tierService.GetTierByIdAsync(model.TierId ?? 0);
				if(tierDetail != null)
				{
                    price = tierDetail.Price * model.Qty;
                    name = tierDetail.Name ?? "";
                }
			
			}

            var user = _idenityService.GetLoggedInUser();




			Guid guid = Guid.NewGuid();



			string ticketId = "T" + guid.ToString("N");

			ticketId = ticketId.Substring(0, 11);



			CreateTicketDTO ticket = new CreateTicketDTO()
			{

				TicketCode = ticketId,
				Qty = model.Qty,
				TotalPrice = price,
				EventId = eventDetail.Id,
				TierName = name,
				UserId = user.UserId ,
				Venue = eventDetail.Venue,
				Date = eventDetail.Date.ToString()

			};

			var res = await _ticketService.CreateTicket(ticket);

			if (res)
			{

				var url = "https://a.khalti.com/api/v2/epayment/initiate/";
				var payload = new
				{
					return_url = "https://localhost:7096/ticketPurchase",
					website_url = "https://localhost:7096/ticketPurchase",
					amount = price ,
					purchase_order_id = ticketId,
					purchase_order_name = name,
					customer_info = new
					{
						name = user.UserName,
						email = "test@khalti.com",
						phone = "9800000001"
					}
				};

				var jsonPayload = JsonConvert.SerializeObject(payload);
				var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Authorization", "key live_secret_key_68791341fdd94846a146f0457ff7b455");
				var response = await client.PostAsync(url, content);
				var responseContent = await response.Content.ReadAsStringAsync();
				Console.WriteLine(responseContent);
				return Ok(responseContent);
			}

				return StatusCode(500, "Failed to create ticket");

		}
	}
}
