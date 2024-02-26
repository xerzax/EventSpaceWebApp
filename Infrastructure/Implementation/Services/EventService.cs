using Application.DTOs;
using Application.DTOs.Event_DTO;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using EventVerse.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class EventService : IEventService
	{
		private readonly IGenericRepository<Event> _eventRepository;
        private readonly IUserIdentityService _userIdentityService;

        public EventService(IGenericRepository<Event> eventRepository, IUserIdentityService userIdentityService)
		{
			_eventRepository = eventRepository;
            _userIdentityService = userIdentityService;
        }

		public async Task<IEnumerable<EventDTO>> GetAllEventAsync()
		{
			var events = await _eventRepository.GetAllAsync();
			var eventDTOs = new List<EventDTO>();

			foreach (var evt in events)
			{
				eventDTOs.Add(new EventDTO
				{
					Id = evt.Id,
					Name = evt.Name,
					Venue = evt.Venue,
					Date = evt.Date,
					Artist = evt.Artist,
					StartingPrice = evt.StartingPrice,
					TotalSeats = evt.TotalSeats,
					SoldSeats = evt.SoldSeats,
					AmountRaised = evt.AmountRaised,
					AvailableSeats = evt.AvailableSeats
				});
			}

			return eventDTOs;
		}
		public async Task UpdateEvent(int id, int qty)
		{
			var evt = await _eventRepository.GetByIdAsync(id);
			if (evt == null) return;
			evt.AvailableSeats -= qty;
			evt.SoldSeats += qty;
			if (evt.AvailableSeats <= 0 || DateTime.Now > evt.Date)
			{
				evt.Status = EventStatus.Closed;
			}
			await _eventRepository.UpdateAsync(evt);
		}

		public async Task UpdateFundAsync(int id, int amount, int qty)
		{
			var evt = await _eventRepository.GetByIdAsync(id);
			if (evt == null) return;
			evt.AvailableSeats -= qty;
			evt.SoldSeats += qty;
			if (evt.AvailableSeats <= 0 || DateTime.Now > evt.Date)
			{
				evt.Status = EventStatus.Closed;
			}
			evt.AmountRaised += amount;
			await _eventRepository.UpdateAsync(evt);
		}

		public async Task<EventDTO> GetEventByIdAsync(int id)
		{
			var evt = await _eventRepository.GetByIdAsync(id);
			if (evt == null) return null;

			return new EventDTO
			{
				Id = evt.Id,
				Name = evt.Name,
				Venue = evt.Venue,
				Date = evt.Date,
				Artist = evt.Artist,
				StartingPrice = evt.StartingPrice,
				TotalSeats = evt.TotalSeats,
				SoldSeats = evt.SoldSeats,
				AmountRaised = evt.AmountRaised,
				AvailableSeats = evt.AvailableSeats
			};
		}

        public async Task<EventRequestDTO> CreateEventByAsync(EventRequestDTO evt)
        {
			var user = _userIdentityService.GetLoggedInUser();
			var newEvent = new Event()
			{
				Name = evt.Name,
				Venue = evt.Venue,
				Artist = evt.Artist,
				Date = evt.Date,
				StartingPrice = evt.StartingPrice,
				TotalSeats = evt.TotalSeats,
				UserId = user.UserId,
			};
			var addedEvent = await _eventRepository.AddAsync(newEvent);
			return new EventRequestDTO
			{
				Name = evt.Name,
				Venue = evt.Venue,
				Artist = evt.Artist,
				Date = evt.Date,
				StartingPrice = evt.StartingPrice,
				TotalSeats = evt.TotalSeats,
			};
        }
    }
}
