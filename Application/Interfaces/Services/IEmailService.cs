using Application.DTOs.Email;
using Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface IEmailService
	{
		Task SendEmail(EmailActionDto emailAction);
		Task SendTicketPurchaseConfirmationEmail(TicketResponseDTO ticketResponseDTO, string userEmail);

	}
}
