using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface ITierService
	{
		Task<TierDTO> GetTierByIdAsync(int id);
		Task UpdateTierAsync(int tierId, int qty);
	}
}
