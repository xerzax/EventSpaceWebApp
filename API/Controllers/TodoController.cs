using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entity.Todo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace API.Controllers
{
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
			try
			{
				var todoById = await _todoService.GetTodoById(id);
				if (todoById == null)
				{
					return NotFound();
				}
				return Ok(todoById);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("PostTodos")]
		public async Task<ActionResult<Todo>> AddTodos([FromBody] TodoDTO todoDTOs)
		{
			try
			{
				var addedTodo = await _todoService.AddTodoAsync(new Todo
				{
					TaskName = todoDTOs.TaskName,
					DueDate = todoDTOs.DueDate,
					IsCompleted = todoDTOs.IsCompleted,
				});
				return CreatedAtAction(nameof(GetTodoById), new { id = addedTodo.Id }, addedTodo);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTodos(int id, [FromBody] TodoDTO todoDTO)
		{
			try
			{
				var toUpdate = await _todoService.GetTodoById(id);
				await _todoService.UpdateTodoAsync(id,toUpdate);
				return Ok(toUpdate);
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

