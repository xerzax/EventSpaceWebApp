using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

		public async Task<Photo> AddPhotosAsync(Photo photos)
		{
			var photoToAdd = new Photo
			{
				CreatedAt = DateTime.Now,
				PhotoName = photos.PhotoName,
				PhotoUrl = photos.PhotoUrl,
				Title = photos.Title,
				Tags = photos.Tags,
			};
			var addedPhotos = await _photoRepository.AddAsync(photoToAdd);
			return addedPhotos;
		}

		public async Task DeletePhotosAsync(int id)
		{
			var toDelete = await _photoRepository.GetByIdAsync(id);
			await _photoRepository.DeleteAsync(toDelete);
		}

		public async Task<IEnumerable<PhotoDTO>> GetAllPhotosAsync()
		{
			var allPhotos = await _photoRepository.GetAllAsync();
			var result = new List<PhotoDTO>();
			foreach (var photo in allPhotos)
			{
				result.Add(new PhotoDTO
				{
					Title = photo.Title,
					Tags = photo.Tags,
				});
			}
			return result.ToList();
		}

		public async Task<PhotoDTO> GetByPhotoIdAsync(int id)
		{
			var photoById = await _photoRepository.GetByIdAsync(id);
			var result = new PhotoDTO()
			{
				Title = photoById.Title,
				Tags = photoById.Tags,
			};
			return result;
		}

		public async Task UpdatePhotosAsync(Photo photos)
		{
			var photoToUpdate = await _photoRepository.GetByIdAsync(photos.Id);
			if (photoToUpdate != null)
			{
				photoToUpdate.Title = photos.Title;
				photoToUpdate.Tags = photos.Tags;
				await _photoRepository.UpdateAsync(photoToUpdate);
			}
		}
	}

}
