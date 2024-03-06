using Application.DTOs;
using Application.DTOs.Todo;
using Domain.Entity.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface ITodoService
	{
		Task<IEnumerable<TodoDTO>> GetAllTodosAsync();
		Task<TodoDTO> GetTodoById(int id);
		Task<Todo> AddTodoAsync(TodoDTO todos);
		Task DeleteTodoAsync(int id);
		Task UpdateTodoAsync(Todo todos);
	}
}
