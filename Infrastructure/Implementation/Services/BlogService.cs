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
	public class BlogRepository : IBlogService
	{
		private readonly IGenericRepository<Blog> _blogRepository;

		public BlogRepository(IGenericRepository<Blog> blogRepository) 
		{
			_blogRepository = blogRepository;
		}
		public async Task<Blog> AddBlogAsync(Blog blog)
		{
			var blogToAdd = new Blog()
			{
				Content = blog.Content,
		
				Title = blog.Title,
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
			var result = new List<BlogDTO>();
			foreach(var blog in allBlogs)
			{
				var getAllBlogs = new BlogDTO()
				{
					Title = blog.Title,
					Content = blog.Content,
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
