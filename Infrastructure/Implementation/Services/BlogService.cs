﻿using Application.DTOs;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Identity.Dependency;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class BlogService : IBlogService
	{
		private readonly IGenericRepository<Blog> _blogRepository;
		private readonly IUserIdentityService _userIdentityService;
		private readonly IFileService _fileService;

		public BlogService(IGenericRepository<Blog> blogRepository, IUserIdentityService userIdentityService, IFileService fileService)
		{
			_blogRepository = blogRepository;
			_userIdentityService = userIdentityService;
			_fileService = fileService;
		}

        public async Task<Blog> AddBlogAsync(BlogDTO blog)
        {
            var user = _userIdentityService.GetLoggedInUser();

            if (user == null)
            {
                throw new Exception("User is not logged in.");
            }

			var blogToAdd = new Blog()
			{
				Content = blog.Content,
				CreatedAt = DateTime.Now,
				PhotoName = blog.PhotoName,
				Title = blog.Title,
				UserId = user.UserId,
            };

            var isAdded = await _blogRepository.AddAsync(blogToAdd);
            
			return isAdded;
        }

        public async Task DeleteBlogAsync(int id)
		{
			var toDelete = await _blogRepository.GetByIdAsync(id);
			await _blogRepository.DeleteAsync(toDelete);
		}

		public async Task<IEnumerable<BlogDTO>> GetAllBlogsAsync()
		{
			var allBlogs = await _blogRepository.GetAllAsync();
			var user = _userIdentityService.GetLoggedInUser();
			var result = new List<BlogDTO>();
			foreach(var blog in allBlogs)
			{
				var getAllBlogs = new BlogDTO()
				{
					Id = blog.Id,
					Title = blog.Title,
					Content = blog.Content,
					PhotoName = blog.PhotoName,
					CreatedAt = blog.CreatedAt
				};
				result.Add(getAllBlogs);
			}
			return result.ToList();
		}

		public async Task<BlogDTO> GetByIdBlogAsync(int id)
		{
			var blogById = await _blogRepository.GetByIdAsync(id);
			var result = new BlogDTO()
			{
				Content = blogById.Content,
				Title = blogById.Title,
				PhotoName= blogById.PhotoName
			};
			return result;
		}

		public async Task UpdateBlogAsync(Blog blog)
		{
			var blogToUpdate = await _blogRepository.GetByIdAsync(blog.Id);
			if(blogToUpdate != null)
			{
				blogToUpdate.Title = blog.Title;
				blogToUpdate.Content = blog.Content;
				await _blogRepository.UpdateAsync(blogToUpdate);
			}
		}
	}
}
