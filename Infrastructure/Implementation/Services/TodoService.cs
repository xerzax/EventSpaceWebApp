﻿using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
	public class TodoService : ITodoService
	{
		private readonly IGenericRepository<Todo> _todoRepository;

		public TodoService(IGenericRepository<Todo> todoRepository)
		{
			_todoRepository = todoRepository;
		}

		public async Task<Todo> AddTodoAsync(Todo todos)
		{
			var todoToAdd = new Todo
			{
				TaskName = todos.TaskName,
				DueDate = todos.DueDate,
				IsCompleted = todos.IsCompleted,
			};
			var isAdded = await _todoRepository.AddAsync(todoToAdd);
			return isAdded;
		}

		public async Task DeleteTodoAsync(int id)
		{
			var toDelete = await _todoRepository.GetByIdAsync(id);
			await _todoRepository.DeleteAsync(toDelete);
		}

		public async Task<IEnumerable<TodoDTO>> GetAllTodosAsync()
		{
			var todos = await _todoRepository.GetAllAsync();
			var result = new List<TodoDTO>();
			foreach (var todo in todos)
			{
				result.Add(new TodoDTO
				{
					TaskName = todo.TaskName,
					DueDate = todo.DueDate,
					IsCompleted = todo.IsCompleted,
				});
			}
			return result;
		}

		public async Task<TodoDTO> GetTodoById(int id)
		{
			var todoById = await _todoRepository.GetByIdAsync(id);
			var result = new TodoDTO()
			{
				TaskName = todoById.TaskName,
				DueDate = todoById.DueDate,
				IsCompleted = todoById.IsCompleted,
			};
			return result;
		}

		public async Task UpdateTodoAsync(int id, TodoDTO todos)
		{
			var toUpdate = await _todoRepository.GetByIdAsync(id);
			if (toUpdate != null)
			{
				toUpdate.TaskName = todos.TaskName;
				toUpdate.DueDate = todos.DueDate;
				toUpdate.IsCompleted = todos.IsCompleted;
				await _todoRepository.UpdateAsync(toUpdate);
			}
		}
	}
}
