using Application.DTOs;
using Application.DTOs.Event_DTO;
using Domain.Entity.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface IEventService
	{
		Task<EventDTO> GetEventByIdAsync(int id);
		Task UpdateEvent(int id, int qty);
		Task UpdateFundAsync(int id, int amount,int qty);
		Task<IEnumerable<EventDTO>> GetAllEventAsync();
		Task<EventRequestDTO> CreateEventByAsync(EventRequestDTO evt);

		Task<List<TicketsSoldByCategory>> GetTicketsSoldByCategory();
        Task<List<EventDTO>> GetTicketsSoldByOrganizer();

        Task<TicketsSoldByEvent> GetTicketsSoldByEvent(int eventId);

        


    }
}
