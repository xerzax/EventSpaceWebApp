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

		/*public async Task<Photo> AddPhotosAsync(Photo photos)
		{
			var user = _userIdentityService.GetLoggedInUser();
			var photoToAdd = new Photo
			{
				
				CreatedAt = DateTime.Now,
				Title = photos.Title,
				Tags = photos.Tags,
				UserId = user.UserId

			};
			var addedPhotos = await _photoRepository.AddAsync(photoToAdd);
			return addedPhotos;
		}*/

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

		public async Task InsertPhoto(PhotoDTO photo)
		{

			var user = _userIdentityService.GetLoggedInUser();
			string imageUploads = "Image";

			var imageUpload = await _fileService.UploadUserFile(photo.Photo, imageUploads);
			if (user == null)
			{
				throw new Exception("User is not logged in.");
			}

			// Explicitly convert the userString to a Guid
			/*Guid userGuid = new Guid(userString);*/

			var photoModel = new Photo
			{
				CreatedAt = DateTime.Now,
				DeletedAt = DateTime.UtcNow.AddDays(7),
				PhotoName = photo.PhotoName,
				Title = photo.Title,
				UserId = user.UserId,
				IsDeleted = false,
				Tags = photo.Tags,
				LastUpdatedAt = DateTime.Now,
			};
			await _photoRepository.AddAsync(photoModel);
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
