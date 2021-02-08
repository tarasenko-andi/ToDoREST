using System;
using System.Collections.Generic;
using System.Text;
using ToDoREST.Models;

namespace ToDoREST.State
{
	public class TodoItem
	{
		public Guid Id { get; set; }
		public string Text { get; set; }
		public int id { get; set; }

		public string username { get; set; }

		public string email { get; set; }

		public string text { get; set; }

		public Status status { get; set; }
	}
	public enum Status { NoExecute, NoExecuteAndAdminCheck, Execute = 10, ExecuteAndAdminCheck = 11 };
}
