using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class FollowService : IFollowService
	{
		private readonly IGenericRepository<UserFollowings> _repository;
		private readonly IUserIdentityService _userIdentityService;

		public FollowService(IGenericRepository<UserFollowings> repository, IUserIdentityService userIdentityService)
		{
			_repository = repository;
			_userIdentityService = userIdentityService;
		}

		public async Task<bool> FollowUser(Guid UserId)
		{
			var user = _userIdentityService.GetLoggedInUser();

			var followUser = new UserFollowings
			{
				FollowerId = user.UserId,
				FollowingId = UserId
			};
			await _repository.AddAsync(followUser);

			return true;

		}
	}
}
