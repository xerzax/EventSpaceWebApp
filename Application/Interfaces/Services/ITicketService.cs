using Application.DTOs.Event_DTO;
using Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface ITicketService
	{
		/*Task<IEnumerable<TicketResponseDTO>> GetAllTickets();
		Task<TicketResponseDTO> GetTicketByIdAsync(int id);
		Task<TicketRequestDTO> CreateTicketByAsync(TicketRequestDTO ticket);*/

		Task<TicketResponseDTO> BuyTicketsAsync(TicketRequestDTO ticketRequestDTO);

		Task<bool> CreateTicket(CreateTicketDTO ticketRequestDTO);

        Task<bool> ConfirmTicket(string code,string email);


        Task<bool> SendFreeticket(CreateTicketDTO ticketRequestDTO);






    }
}
