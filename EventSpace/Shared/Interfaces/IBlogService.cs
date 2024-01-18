using EventSpace.Shared.DTOs;
using EventSpace.Shared.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSpace.Shared.Interfaces
{
	public interface IBlogService
	{
		Task<IEnumerable<BlogDTO>> GetAllBlogAsync();
		Task<BlogDTO> GetBlogByIdAsync(int id);
		Task DeleteBlogByIdAsync(int id);
		Task UpdateBlogAsync(Blog blog);
		Task<Blog> AddBlogAsync(Blog blog);
	}
}
