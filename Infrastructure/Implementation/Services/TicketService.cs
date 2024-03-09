using Application.DTOs.Ticket;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using Domain.Entity.Ticket;
using EventSpaceApi.Domain.Entity.Identity;
using Infrastructure.Implementation.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class TicketService : ITicketService
	{
		private readonly IGenericRepository<Ticket> _ticketRepo;
		private readonly IUserIdentityService _userIdentity;
		private readonly IGenericRepository<Event> _eventRepo;
		private readonly UserManager<User> _userManager;
		private readonly IEmailService _emailService;

		public TicketService(IGenericRepository<Ticket> ticketRepo, IUserIdentityService userIdentity, IGenericRepository<Event> eventRepo, UserManager<User> userManager, IEmailService emailService)
		{
			_ticketRepo = ticketRepo;
			_userIdentity = userIdentity;
			_eventRepo = eventRepo;
			_userManager = userManager;
			_emailService = emailService;
		}

		public async Task<TicketResponseDTO> BuyTicketsAsync(TicketRequestDTO ticketRequestDTO)
		{
			var user = _userIdentity.GetLoggedInUser();

			var @event = await _eventRepo.GetByIdAsync(ticketRequestDTO.EventId);

			var appUser = await _userManager.FindByIdAsync(user.UserId.ToString());

			var totalPrice = ticketRequestDTO.Qty * @event.StartingPrice;

			if (@event.AvailableSeats >= ticketRequestDTO.Qty && @event.AvailableSeats > 0)
			{
				@event.AvailableSeats -= ticketRequestDTO.Qty;
				@event.SoldSeats += ticketRequestDTO.Qty;

				await _eventRepo.UpdateAsync(@event);

				var purchaseTicket = new Ticket
				{
					UserId = user.UserId,
					EventId = ticketRequestDTO.EventId,
					Qty = ticketRequestDTO.Qty,
					Eventdate = @event.Date.ToString(),
					TicketType = @event.Type,
					TierName = ticketRequestDTO.TierName,
					TotalPrice = (int)totalPrice,
					Venue = @event.Venue,
				};

				await _ticketRepo.AddAsync(purchaseTicket);

				var ticketResponse = new TicketResponseDTO
				{
					Qty = purchaseTicket.Qty,
					TotalPrice = purchaseTicket.TotalPrice,
					isConfirmed = purchaseTicket.isConfirmed,
					TierName = purchaseTicket.TierName,
					TicketType = purchaseTicket.TicketType,
					Venue = purchaseTicket.Venue,
					Eventdate = purchaseTicket.Eventdate
				};
				await _emailService.SendTicketPurchaseConfirmationEmail(ticketResponse, appUser.Email);
				return ticketResponse;
			}
			else
			{
				throw new Exception("Sakkyo ticket khaiss");
			}


		}

		/*public async Task<TicketRequestDTO> CreateTicketByAsync(TicketRequestDTO ticket)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<TicketResponseDTO>> GetAllTickets()
		{
			throw new NotImplementedException();
		}

		public async Task<TicketResponseDTO> GetTicketByIdAsync(int id)
		{
			throw new NotImplementedException();
		}*/

	}
}
