using Application.DTOs;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface IBlogService
	{
		Task<IEnumerable<BlogDTO>> GetAllBlogsAsync();
		Task<BlogDTO> GetByIdBlogAsync(int id);
		Task<Blog> AddBlogAsync(BlogDTO blog);
		Task UpdateBlogAsync(Blog blog);
		Task DeleteBlogAsync(int id);
	}
}
