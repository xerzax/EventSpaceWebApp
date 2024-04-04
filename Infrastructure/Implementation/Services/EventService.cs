using Application.DTOs;
using Application.DTOs.Event_DTO;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using Domain.Entity.Post;
using EventVerse.Core.Enums;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IGenericRepository<Tier> _tierRepository;
		private readonly IFileService _fileService;
		private readonly IGenericRepository<Event> _eventRepository;

		private readonly IUserIdentityService _userIdentityService;

		public EventService(IGenericRepository<Event> eventRepository, IUserIdentityService userIdentityService, IGenericRepository<Tier> tierRepository, IFileService fileService)
		{
			_eventRepository = eventRepository;
			_userIdentityService = userIdentityService;
			_tierRepository = tierRepository;
			_fileService = fileService;
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

			var tiers = await _tierRepository.Where(x => x.EventId == id);

			List<TierDTO> tierDTOs = new List<TierDTO>();

			if (tiers.Count > 0)
			{

				tierDTOs = MapToDTO(tiers);
			}




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
				AvailableSeats = evt.AvailableSeats,
				TierList = tierDTOs
			};
		}

		private List<TierDTO> MapToDTO(List<Tier> entities)
		{
			return entities.Select(entity => MapToDTO(entity)).ToList();
		}

		private TierDTO MapToDTO(Tier entity)
		{
			return new TierDTO
			{
				Id = entity.Id,
				Name = entity.Name,
				TotalSeats = entity.TotalSeats,
				AvailableSeats = entity.AvailableSeats,
				Price = entity.Price,
			};
		}

		public async Task<EventRequestDTO> CreateEventByAsync(EventRequestDTO evt)
		{
			var user = _userIdentityService.GetLoggedInUser();
			
			if (user == null)
			{
				throw new Exception("User is not logged in.");
			}
			var newEvent = new Event()
			{
				Name = evt.Name,
				Venue = evt.Venue,
				Artist = evt.Artist,
				Date = evt.Date,
				StartingPrice = evt.StartingPrice,
				TotalSeats = evt.TotalSeats,
				AvailableSeats = evt.TotalSeats,
				UserId = user.UserId,
			};
			var addedEvent = await _eventRepository.AddAsync(newEvent);

			foreach (var tier in evt.TierList)
			{
				Tier tierObj = new Tier()
				{
					Name = tier.Name,
					Price = tier.Price,
					TotalSeats = tier.TotalSeats,
					EventId = addedEvent.Id,
					AvailableSeats = tier.TotalSeats,

				};

				var addedTier = await _tierRepository.AddAsync(tierObj);

			}

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