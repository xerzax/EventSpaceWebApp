using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class TierService : ITierService
	{
		private readonly IGenericRepository<Tier> _tierRepository;

		public TierService(IGenericRepository<Tier> tierRepository)
		{
			_tierRepository = tierRepository;
		}

		public async Task<TierDTO> GetTierByIdAsync(int id)
		{
			var tier = await _tierRepository.GetByIdAsync(id);
			if (tier == null) return null;
			return new TierDTO
			{
				Id = tier.Id,
				Name = tier.Name,
				TotalSeats = tier.TotalSeats,
				AvailableSeats = tier.AvailableSeats,
				Price = tier.Price,
				AmountRaised = tier.AmountRaised,
				EventId = tier.EventId,
			};
		}

		public async Task UpdateTierAsync(int tierId, int qty)
		{
			var tier = await _tierRepository.GetByIdAsync(tierId);
			if (tier == null) return;
			var payableAmount = qty * tier.Price;
			tier.AvailableSeats -= qty;
			tier.SoldSeats += qty;
			tier.AmountRaised += payableAmount;
			await _tierRepository.UpdateAsync(tier);
		}
	}

}
