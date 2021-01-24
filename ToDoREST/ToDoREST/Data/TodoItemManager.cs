using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoREST.Models;
using static ToDoREST.Constants;

namespace ToDoREST.Data
{
	public class TodoItemManager
	{
		IRestService restService;

		public TodoItemManager(IRestService service)
		{
			restService = service;
		}

		public Task<(int pagesCount, List<TodoItem>)> GetTasksAsync(int page, SortedField sortedField, SortDirection sortDirection)
		{
			return restService.RefreshDataAsync(page, sortedField, sortDirection);
		}

		public Task<bool> SaveTaskAsync(TodoItem item, bool isNewItem = false)
		{
			return restService.SaveTodoItemAsync(item, isNewItem);
		}

		public Task<bool> Logining(string login, string password)
        {
			return restService.Logining(login, password);
        }
		public Task<bool> UpdateTodoItemAsync(TodoItem item)
        {
			return restService.UpdateTodoItemAsync(item);
        }
	}
}
