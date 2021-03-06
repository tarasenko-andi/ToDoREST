﻿using System;
using System.Collections.Generic;
using System.Text;
using ToDoREST.State;

namespace ToDoREST.Models
{
	public class TodoItem2
	{
		public Guid Id { get; set; }
		public int id { get; set; }

		public string username { get; set; }

		public string email { get; set; }

		public string text { get; set; }

		public Status status { get; set; }
	}
}
