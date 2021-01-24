using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoREST.Models;
using static ToDoREST.Constants;

namespace ToDoREST.Data
{
	public interface IRestService
	{
		Task<(int pagesCount, List<TodoItem>)> RefreshDataAsync(int page, SortedField sortedField, SortDirection sortDirection);

		Task SaveTodoItemAsync(TodoItem item, bool isNewItem);

		Task<bool> Logining(string login, string password);
	}
}
