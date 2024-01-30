using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly IGenericRepository<Photo> _photoRepository;

		public PhotoService(IGenericRepository<Photo> photoRepository)
		{
			_photoRepository = photoRepository;
		}

		public async Task<Photo> AddPhotos(Photo photos)
		{
			
		}

		public Task DeletePhotosAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<PhotoDTO>> GetAllPhotosAsync()
		{
			throw new NotImplementedException();
		}

		public Task<PhotoDTO> GetByBlogIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdatePhotosAsync(Photo photos)
		{
			throw new NotImplementedException();
		}
	}

}
