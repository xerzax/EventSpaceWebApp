using Application.DTOs;
using Application.DTOs.Ticket;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using EventVerse.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
    public class DonationService : IDonationService
    {
        private readonly IGenericRepository<Donation> _donationRepository;
        private readonly IGenericRepository<Event> _eventRepository;


        private readonly IUserIdentityService _userIdentityService;
        public DonationService(IGenericRepository<Donation> donationRepository, IUserIdentityService userIdentityService, IGenericRepository<Event> eventRepository)
        {
            _donationRepository = donationRepository;
            _userIdentityService = userIdentityService;
            _eventRepository = eventRepository;
        }

        public async Task<DonationResponseDTO> CreateDonation(DonationRequestDTO model)
        {
            try
            {
                var user = _userIdentityService.GetLoggedInUser();

                if (user == null)
                {
                    throw new Exception("User is not logged in.");
                }
                var donation = new Donation()
                {
                    EventId = model.EventId,
                    AmountDonated = model.Amt,
                    UserId = user.UserId,
                    CreatedAt = DateTime.UtcNow,
                };
                var res = await _donationRepository.AddAsync(donation);

                var eventName = await _eventRepository.GetByIdAsync(res.EventId);

                var result = new DonationResponseDTO
                {
                    AmountDonated = res.AmountDonated,
                    EventName = eventName.Name

                };
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
