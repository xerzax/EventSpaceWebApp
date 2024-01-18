using EventSpace.Server.Data;
using EventSpace.Shared.Entities.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventSpace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PostController(ApplicationDbContext context = null)
        {
            _context = context;
        }

        [HttpGet("GetBlogs")]
        public ActionResult<IEnumerable<Blog>> GetAllBlogs()
        {
            return Ok(_context.Blog.ToList());
        }

        [HttpGet("GetBlogById/{id}")]
        public ActionResult<Blog> GetBlogById(int id)
        {
            var blogById = _context.Blog.FirstOrDefault(e => e.Id == id);
            if (blogById == null)
                return NotFound();
            return Ok(blogById);
        }

        [HttpPost("CheckBlog")]
		public ActionResult CheckMethod([FromBody] Blog blog)
		{
			return Ok("abc");

			return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);
		}


		[HttpPost("PostBlog")]
        public ActionResult<Blog> PostBlog([FromForm]Blog blog)
        {
            _context.Blog.Add(blog);
            _context.SaveChanges();

			return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);
		}

		[HttpGet("GetPhotos")]
        public ActionResult<IEnumerable<Photo>> GetAllPhotos()
        {
            return Ok(_context.Photo.ToList());
        }

		[HttpGet("GetPhotoById/{id}")]

		public ActionResult<Photo> GetPhotoById(int id)
        {
            var photoById = _context.Photo.FirstOrDefault(e => e.Id == id);
            if(photoById == null)
            {
                return NotFound();
            }
            return Ok(photoById);
        }

        [HttpPost("PostPhoto")]
        public ActionResult<Photo> PostPhoto([FromForm]Photo photo)
        {
            _context.Photo.Add(photo);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetBlogById), new { id = photo.Id }, photo);
        }

    }
}
