﻿using Application.DTOs;
using Application.DTOs.Event_DTO;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using Domain.Entity.Post;
using EventVerse.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

				var evttype = (EventType)evt.Type;
				var name = Enum.GetName(typeof(EventType), evttype); 

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
					AvailableSeats = evt.AvailableSeats,
					EventPhoto = evt.EventPhoto,
					Type = name

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



			var evttype = (EventType)evt.Type;
			var name = Enum.GetName(typeof(EventType), evttype); 

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
				TierList = tierDTOs,
				Type = name
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
				EventPhoto = evt.EventPhoto,
				UserId = user.UserId,
				Type = (EventType)evt.EventType
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
				EventPhoto= evt.EventPhoto,

			};
		}

      
        public async Task<List<TicketsSoldByCategory>> GetTicketsSoldByCategory()
        {
            var eventsQueryable = await _eventRepository.GetAllAsync();

            var eventsSoldByCategory = eventsQueryable
                .GroupBy(e => e.Type)
                .Select(g => new TicketsSoldByCategory
                {
                    CategoryName = Enum.GetName(g.Key.GetType(), g.Key),
                    TicketsSold = g.Sum(e => e.SoldSeats)
                });

            return eventsSoldByCategory.ToList();
        }

        public async Task<TicketsSoldByEvent> GetTicketsSoldByEvent(int eventId)
        {
            var eventsQueryable = await _eventRepository.GetByIdAsync(eventId);

			return new TicketsSoldByEvent()
			{
				Name = eventsQueryable.Name,
				TicketsSold = eventsQueryable.SoldSeats,
				AvailableTickets = eventsQueryable.AvailableSeats
			};


        }

        public async Task<List<EventDTO>> GetTicketsSoldByOrganizer()
        {
			var user = _userIdentityService.GetLoggedInUser();
            var eventDTOs = new List<EventDTO>();

            var eventsQueryable = await _eventRepository.Where(x=> x.UserId == user.UserId);
            foreach (var evt in eventsQueryable)
            {

                var evttype = (EventType)evt.Type;
                var name = Enum.GetName(typeof(EventType), evttype);

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
                    AvailableSeats = evt.AvailableSeats,
                    EventPhoto = evt.EventPhoto,
                    Type = name

                });
            }

            return eventDTOs;



        }
    }
}