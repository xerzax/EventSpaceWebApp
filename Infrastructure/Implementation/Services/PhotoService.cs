using Application.DTOs;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Identity.Dependency;
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
		private readonly IUserIdentityService _userIdentityService;
		private readonly IFileService _fileService;
		private readonly IGetUserByID _getUserByID;

		public PhotoService(IGenericRepository<Photo> photoRepository, IUserIdentityService userIdentityService, IFileService fileService, IGetUserByID getUserByID)
		{
			_photoRepository = photoRepository;
			_userIdentityService = userIdentityService;
			_fileService = fileService;
			_getUserByID = getUserByID;
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
					PhotoName = photo.PhotoName
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

		public async Task<Photo> InsertPhoto(PhotoDTO photo)
		{
			var user = _userIdentityService.GetLoggedInUser();

			if (user == null)
			{
				throw new Exception("User is not logged in.");
			}

			var photoModel = new Photo()
			{
				
				CreatedAt = DateTime.Now,
				PhotoName = photo.PhotoName,
				Tags = photo.Tags,
				Title = photo.Title,
				UserId = user.UserId,
			};

			var isAdded = await _photoRepository.AddAsync(photoModel);
			return isAdded;
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
