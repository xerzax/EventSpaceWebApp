﻿using Application.DTOs;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface IPhotoService
	{
		Task<IEnumerable<PhotoDTO>> GetAllPhotosAsync();
		Task<PhotoDTO> GetByPhotoIdAsync(int id);
		Task<Photo> AddPhotosAsync(Photo photos);
		Task DeletePhotosAsync(int id);
		Task UpdatePhotosAsync(Photo photos);
	}
}