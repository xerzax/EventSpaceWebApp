using EventSpace.Shared.DTOs;
using EventSpace.Shared.Entities.Post;
using EventSpace.Shared.Interfaces;

namespace EventSpace.Server.Services
{
	public class BlogService : IBlogService
	{
		private readonly IGenericRepository<Blog> _blogRepository;

		public BlogService(IGenericRepository<Blog> blogRepository) 
		{
			_blogRepository = blogRepository;
		}

		public async Task<Blog> AddBlogAsync(Blog blog)
		{
			var blogToAdd = new Blog()
			{
				Content = blog.Content,
				CreatedAt = DateTime.Now,
				PhotoName = blog.PhotoName,
				PhotoUrl = blog.PhotoUrl,
				Title = blog.Title,
			};
			var isAdded = await _blogRepository.AddAsync(blogToAdd);
			return isAdded;
		}

		public async Task DeleteBlogByIdAsync(int id)
		{
			var toDelete = await _blogRepository.GetByIdAsync(id);
			await _blogRepository.DeleteAsync(toDelete);
		}

		public async Task<IEnumerable<BlogDTO>> GetAllBlogAsync()
		{
			var allBlogs = await _blogRepository.GetAllAsync();
			var result = new List<BlogDTO>();
			foreach (var blog in allBlogs)
			{
				var getAllBlogs = new BlogDTO()
				{
					Content = blog.Content,
					Title = blog.Title,
				};
				result.Add(getAllBlogs);
			}
			return result.ToList();
		}

		public async Task<BlogDTO> GetBlogByIdAsync(int id)
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
			if (blogToUpdate != null)
			{
				blogToUpdate.Content = blog.Content;
				blogToUpdate.Title = blog.Title;
				await _blogRepository.UpdateAsync(blogToUpdate);
			}
		}
	}
}
