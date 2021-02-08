using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoREST.Actions
{
    internal class RemoveTodoAction : IAction
    {
        public Guid Id { get; internal set; }
    }
}
