using Application.DTOs.Event;
using Application.DTOs.User;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using Domain.Entity.Ticket;
using EventSpaceApi.Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Implementation.Services.DashboardService;

namespace Infrastructure.Implementation.Services
{

    public class DashboardService : IDashboardService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<Donation> _donationRepository;

        public DashboardService(UserManager<User> userManager, RoleManager<Role> roleManager, IGenericRepository<Ticket> ticketRepository, IGenericRepository<Event> eventRepository, IGenericRepository<Donation> donationRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _donationRepository = donationRepository;
        }
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            var result = usersInRole.Select(user => new UserResponseDTO
            {
                Email = user.Email,
                Address = user.Address ?? "",
                FullName = user.Name ?? "",
                UserName = user.UserName,
                Contact = user.PhoneNumber,
                IsConfirmed = user.EmailConfirmed
            });

            return result;
        }

        public async Task<IEnumerable<DashboardEventResponseDTO>> GetEventDetails()
        {
            var events = await _eventRepository.GetAllAsync();

            var eventDetails = new List<DashboardEventResponseDTO>();

            foreach (var evt in events)
            {
                var totalAmountRaised = await TotalAmountRaisedForEvent(evt.Id);
                var totalDonatedAmount = await TotalAmountDonatedForEvent(evt.Id);
                var eventDetail = new DashboardEventResponseDTO
                {
                    Artist = evt.Artist,
                    Name = evt.Name,
                    TotalAmountRaised = totalAmountRaised,
                    TotalTicketsSold = evt.SoldSeats,
                    Venue = evt.Venue,
                    DonatedAmount = totalDonatedAmount,
                    EventDate = evt.Date,
                    RemainingSeats = evt.AvailableSeats
                };
                eventDetails.Add(eventDetail);
            }
            return eventDetails;
        }

        private async Task<decimal> TotalAmountRaisedForEvent(int eventId)
        {
            var tickets = await _ticketRepository.GetAllAsync();

            var eventTickets = tickets.Where(x => x.EventId == eventId);

            var totalAmountRaised = eventTickets.Sum(t => t.TotalPrice);

            return totalAmountRaised;
        }

        private async Task<decimal> TotalAmountDonatedForEvent(int eventId)
        {
            var donations = await _donationRepository.GetAllAsync();

            var eventDonations = donations.Where(x => x.EventId == eventId);

            var totalDonationRaised = eventDonations.Sum(t => t.AmountDonated);

            return totalDonationRaised;
        }
    }
}

