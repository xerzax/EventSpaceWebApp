using Application.DTOs;
using Application.DTOs.Ticket;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using Domain.Entity.Ticket;
using EventSpaceApi.Domain.Entity.Identity;
using EventVerse.Core.Enums;
using Infrastructure.Implementation.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace Infrastructure.Implementation.Services
{
    public class TicketService : ITicketService
    {
        private readonly IGenericRepository<Tier> _tierRepo;

        private readonly IGenericRepository<Ticket> _ticketRepo;
        private readonly IUserIdentityService _userIdentity;
        private readonly IGenericRepository<Event> _eventRepo;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IEventService _eventService;






        public TicketService(IGenericRepository<Ticket> ticketRepo, IUserIdentityService userIdentity, IGenericRepository<Event> eventRepo, UserManager<User> userManager, IEmailService emailService, IGenericRepository<Tier> tierRepo, IEventService eventService)
        {
            _ticketRepo = ticketRepo;
            _userIdentity = userIdentity;
            _eventRepo = eventRepo;
            _userManager = userManager;
            _emailService = emailService;
            _tierRepo = tierRepo;
            _eventService = eventService;
        }

        public async Task<TicketResponseDTO> BuyTicketsAsync(TicketRequestDTO ticketRequestDTO)
        {
            var user = _userIdentity.GetLoggedInUser();

            var appUser = await _userManager.FindByIdAsync(user.UserId.ToString());
            var @event = await _eventRepo.GetByIdAsync(ticketRequestDTO.EventId);


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
                throw new Exception("Tickets are no longer available!");
            }


        }

        public async Task<bool> ConfirmTicket(string code,string email)
        {
			var user = _userIdentity.GetLoggedInUser();

			var appUser = await _userManager.FindByIdAsync(user.UserId.ToString());

			var ticket = await _ticketRepo.GetFirstOrDefault(x => x.TicketCode == code);
            if (ticket == null)
            {
                return false;
            }

            ticket.PaymentStatus = "Paid";
            await _ticketRepo.UpdateAsync(ticket);


            var eventDetail = await _eventRepo.GetByIdAsync(ticket.EventId ?? 0);
            if (eventDetail != null)
            {
                eventDetail.SoldSeats += ticket.Qty;

                eventDetail.AvailableSeats -= eventDetail.SoldSeats;

                await _eventRepo.UpdateAsync(eventDetail);

            }




            if (!String.IsNullOrEmpty(ticket.TierName))
            {
                var tier = await _tierRepo.GetFirstOrDefault(x => x.Name == ticket.TierName && x.EventId == ticket.EventId);
                if (tier != null)
                {

                    tier.SoldSeats += ticket.Qty;

                    tier.AvailableSeats -= tier.SoldSeats;

                    await _tierRepo.UpdateAsync(tier);
                }

            }

			var ticketResponse = new TicketResponseDTO
			{
				Qty = ticket.Qty,
				TotalPrice = ticket.TotalPrice,
				isConfirmed = ticket.isConfirmed, 
				TierName = ticket.TierName,
				TicketType = EventType.Paid,
				Venue = ticket.Venue,
				Eventdate = ticket.Eventdate
			};
            if(!String.IsNullOrEmpty(email) && email !="null")
            {
                await _emailService.SendTicketPurchaseConfirmationEmail(ticketResponse, email);

            }
            else
            {
                await _emailService.SendTicketPurchaseConfirmationEmail(ticketResponse, appUser.Email);

            }

            return true;
        }

        public async Task<bool> CreateTicket(CreateTicketDTO ticketRequestDTO)
        {
            try
            {
                var ticket = MapFromDTO(ticketRequestDTO);

                var res = await _ticketRepo.AddAsync(ticket);
                if (res != null)
                {
                    return true;
                }

                return false;

            }
            catch
            {
                return false;
            }




        }

        public async Task<bool> SendFreeticket(CreateTicketDTO ticketRequestDTO)
        {
            var user = _userIdentity.GetLoggedInUser();

            var userId = user.UserId;

            var userDetail = _userIdentity.GetUserDetail(userId);








            var eventDetail = await _eventRepo.GetByIdAsync(ticketRequestDTO.EventId);


            if (eventDetail != null)
            {
                eventDetail.SoldSeats += 1;

                eventDetail.AvailableSeats -= eventDetail.SoldSeats;

                await _eventRepo.UpdateAsync(eventDetail);

            }



            TicketResponseDTO ticketResponseDTO = new TicketResponseDTO()
            {
                Qty = 1,
                Eventdate = eventDetail.Date.ToShortDateString(),
                TotalPrice = 0,
                isConfirmed = true,
                Venue = eventDetail.Venue,
                TicketType = EventType.Free,
            };

            await _emailService.SendTicketPurchaseConfirmationEmail(ticketResponseDTO, userDetail.Email);
            return true;

        }

        private Ticket MapFromDTO(CreateTicketDTO entity)
        {
            return new Ticket
            {
                Qty = entity.Qty,
                UserId = entity.UserId,
                EventId = entity.EventId,
                TierName = entity.TierName,
                TotalPrice = entity.TotalPrice,
                TicketCode = entity.TicketCode,
                Venue = entity.Venue,
                Eventdate = entity.Date,

            };
        }

    }
}

