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
using Domain.Entity.Comment;
using Domain.Entity.Follow;

namespace Infrastructure.Implementation.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IGenericRepository<Photo> _photoRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IFileService _fileService;
        private readonly IGetUserByID _getUserByID;
        private readonly IGenericRepository<UserFollowings> _followRepository;

        public PhotoService(IGenericRepository<Photo> photoRepository, IUserIdentityService userIdentityService, IFileService fileService, IGetUserByID getUserByID, IGenericRepository<Comment> commentRepository, IGenericRepository<UserFollowings> followRepository)
        {
            _photoRepository = photoRepository;
            _userIdentityService = userIdentityService;
            _fileService = fileService;
            _getUserByID = getUserByID;
            _commentRepository = commentRepository;
            _followRepository = followRepository;
        }
        public async Task DeletePhotosAsync(int id)
        {
            var toDelete = await _photoRepository.GetByIdAsync(id);
            await _photoRepository.DeleteAsync(toDelete);
        }

        public async Task<IEnumerable<PhotoDetailsDTO>> GetAllPhotosAsync()
        {
			var result = new List<PhotoDetailsDTO>();

			try
			{
                var loggedInUser = _userIdentityService.GetLoggedInUser();
                var allPhotos = await _photoRepository.GetAllAsync();

                foreach (var photo in allPhotos)
                {
                    var comments = await _commentRepository.GetAllAsync();
                    comments = comments.Where(x => x.PostType == "Photo" && x.PhotoId == photo.Id).ToList();


                    UserFollowings followRecord;

                    if(loggedInUser != null)
                    {
						followRecord = await _followRepository.GetFirstOrDefault(x => (x.FollowerId == photo.UserId && x.FollowingId == loggedInUser.UserId)
																		|| (x.FollowingId == photo.UserId && x.FollowerId == loggedInUser.UserId));

					}
                    else
                    {
						followRecord = await _followRepository.GetFirstOrDefault(x => (x.FollowerId == photo.UserId) || (x.FollowingId == photo.UserId));
					}

                    result.Add(new PhotoDetailsDTO
                    {
                        Id = photo.Id,
                        Title = photo.Title,
                        Tags = photo.Tags,
                        PhotoName = photo.PhotoName,
                        CreatedAt = photo.CreatedAt,
                        CreatedById = photo.UserId,
                        CreatedBy = _userIdentityService.GetUserDetails(photo.UserId).Item1,
                        IsFollowedByUser = followRecord != null,
                        PhotoComments = comments.Select(x => new PhotoComment()
                        {
                            Comment = x.Content,
                            CommentedBy = _userIdentityService.GetUserDetails(x.UserId).Item1,
                            CommentedUserImageUrl = _userIdentityService.GetUserDetails(x.UserId).Item2
                        }).ToList()
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
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