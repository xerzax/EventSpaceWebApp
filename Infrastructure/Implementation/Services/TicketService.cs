using Application.DTOs.Ticket;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class TicketService : ITicketService
	{
		private readonly IGenericRepository<Ticket> _genericRepository;
		private readonly IUserIdentityService _userIdentity;

		public TicketService(IGenericRepository<Ticket> genericRepository, IUserIdentityService userIdentity)
		{
			_genericRepository = genericRepository;
			_userIdentity = userIdentity;
		}

		public async Task<TicketRequestDTO> CreateTicketByAsync(TicketRequestDTO ticket)
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
		}
	}
}
