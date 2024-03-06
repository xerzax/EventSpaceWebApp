using Application.DTOs;
using Application.DTOs.Todo;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Domain.Entity.Todo;
using Infrastructure.Implementation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("GetTodos")]
        public async Task<IActionResult> GetAllTodos()
        {
            try
            {
                var allTodos = await _todoService.GetAllTodosAsync();
                return Ok(allTodos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetTodoById")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var todos = await _todoService.GetTodoById(id);
            if (todos == null)
            {
                return NotFound();
            }
            return Ok(todos);
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost("PostTodos")]
        public async Task<IActionResult> AddTodos([FromBody] TodoDTO todoDTOs)
        {
            var createdTodos = await _todoService.AddTodoAsync(todoDTOs);
            return Ok(createdTodos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodos(int id, [FromBody] Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }
            try
            {
                await _todoService.UpdateTodoAsync(todo);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodos(int id)
        {
            try
            {
                await _todoService.DeleteTodoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

