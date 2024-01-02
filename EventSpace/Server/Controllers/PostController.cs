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
        public ActionResult<IEnumerable<Blog>> Get()
        {
            return Ok(_context.Blog.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Blog> GetBlog(int id)
        {
            var employee = _context.Blog.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }


        [HttpPost]
        public ActionResult<Blog> PostBlog(Blog blog)
        {
            _context.Blog.Add(blog);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        }
    }
}
