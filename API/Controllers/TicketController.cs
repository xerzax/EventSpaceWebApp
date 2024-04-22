using Application.DTOs;
using Application.DTOs.Ticket;
using Application.Interfaces.Identity;
using Application.Interfaces.Services;
using Domain.Entity.Ticket;
using Infrastructure.Implementation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using ZXing.QrCode.Internal;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TicketController : ControllerBase
	{
		private readonly IEmailService _emailService;

		private readonly ITicketService _ticketService;
		private readonly IEventService _eventService;

        private readonly IDonationService _donationService;

        private readonly IUserIdentityService _idenityService;

		private readonly ITierService _tierService;
		private readonly IQRCodeGeneratorService _qrCoder;




        public TicketController(ITicketService ticketService, IEventService eventService, IUserIdentityService idenityService, ITierService tierService, IQRCodeGeneratorService qrCoder, IEmailService emailService, IDonationService donationService)
        {
            _ticketService = ticketService;
            _eventService = eventService;
            _idenityService = idenityService;
            _tierService = tierService;
            _qrCoder = qrCoder;
            _emailService = emailService;
            _donationService = donationService;
        }

        [Authorize]
		[HttpPost("buy-tickets")]
		public async Task<IActionResult> BuyTicketAsync(TicketRequestDTO ticketRequest)
		{
			var result = await _ticketService.BuyTicketsAsync(ticketRequest);
			return Ok(result);
		}

		[HttpGet("GenerateQRCheck")]


		public async Task<IActionResult> GenerateQRCheck()
		{



			CreateTicketDTO ticket = new CreateTicketDTO()
			{

				TicketCode = "t123",
				Qty = 2,
				TotalPrice = 200,
				EventId = 1,
				TierName = "Abc",
				UserId = new Guid(),
				Venue = "Ktm",
				Date = "2024-10-9=09"

			};

			string ticketInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(ticket);
			byte[] qrAsBytes = _qrCoder.GenerateQRCode(ticketInfoJson);
			string base64Image = Convert.ToBase64String(qrAsBytes);


			//string imageHtml = $"<img src='data:image/png;base64,{base64Image}' alt='QR Code' height='300' width='300'>";
			//string imageHtml = $"<img src='data:image/png;base64,{base64Image}' alt='QR Code' height='300' width='300'>";

			await _emailService.SendQrCheck("ritikashrestha426@gmail.com", base64Image);

			return Ok();



		}





		[HttpPost("Confirm/{ticketCode}/{email}")]
        public async Task<IActionResult> Confirm(string ticketCode,string email)
		{
			var res = await _ticketService.ConfirmTicket(ticketCode,email);
			return Ok(res);


		}

        [HttpPost("Donate")]
        public async Task<IActionResult> Payment(DonationRequestDTO model)
		{
            var eventDetail = await _eventService.GetEventByIdAsync(model.EventId);
            var user = _idenityService.GetLoggedInUser();
            string email = "test@khalti.com";
            Guid guid = Guid.NewGuid();



            string ticketId = "D" + guid.ToString("N");
            string returnUrl = "https://localhost:7096/donation";


            ticketId = ticketId.Substring(0, 11);
			var res = await _donationService.CreateDonation(model);;
			if (res !=null)
			{


				var url = "https://a.khalti.com/api/v2/epayment/initiate/";
				var payload = new
				{
					return_url = returnUrl,
					website_url = returnUrl,
					amount = model.Amt * 100,
					purchase_order_id = ticketId,
					purchase_order_name = "Donation",
					customer_info = new
					{
						name = user.UserName,
						email = email,
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

			string email = "test@khalti.com";

			var res = await _ticketService.CreateTicket(ticket);
            string returnUrl = "https://localhost:7096/ticketPurchase";


            if (!String.IsNullOrEmpty(model.Email))
			{
				email = model.Email;
				returnUrl = $"https://localhost:7096/giftTicket/{email}";

            }




			if (res)
			{

				var url = "https://a.khalti.com/api/v2/epayment/initiate/";
				var payload = new
				{
					return_url = returnUrl,
					website_url = returnUrl,
					amount = price * 100,
					purchase_order_id = ticketId,
					purchase_order_name = name,
					customer_info = new
					{
						name = user.UserName,
						email = email,
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

		[HttpPost("FreeTicket")]
		public async Task<IActionResult> FreeTicket(PaymentRequestDTO model)
		{
			EventDTO eventDetail = new EventDTO();

		
			eventDetail = await _eventService.GetEventByIdAsync(model.EventId);



		

			var user = _idenityService.GetLoggedInUser();




			Guid guid = Guid.NewGuid();



			string ticketId = "T" + guid.ToString("N");

			ticketId = ticketId.Substring(0, 11);



			CreateTicketDTO ticket = new CreateTicketDTO()
			{

				TicketCode = ticketId,
				Qty = 1,
				TotalPrice = 0,
				EventId = eventDetail.Id,
				UserId = user.UserId,
				Venue = eventDetail.Venue,
				Date = eventDetail.Date.ToString()

			};


			var res = await _ticketService.CreateTicket(ticket);
			return Ok(res);

		}
	}
}
