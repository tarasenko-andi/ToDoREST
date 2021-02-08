using System;
using System.Collections.Generic;
using System.Text;
using ToDoREST.Models;
using ToDoREST.State;

namespace ToDoREST.Actions
{
    public class FetchTodosAction : IAction
    {
        public IEnumerable<TodoItem> Todos { get; internal set; }
    }
}
