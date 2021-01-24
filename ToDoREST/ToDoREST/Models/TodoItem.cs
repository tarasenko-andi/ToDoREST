using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoREST.Models
{
	public class TodoItem
	{
		public int id { get; set; }

		public string username { get; set; }

		public string email { get; set; }

		public string text { get; set; }

		public bool status { get; set; }
	}
}
