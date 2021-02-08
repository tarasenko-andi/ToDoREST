using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoREST.Actions
{
    internal class AddTodoAction : IAction
    {
        public string Text { get; internal set; }
        public Guid Id { get; internal set; }
    }
}
