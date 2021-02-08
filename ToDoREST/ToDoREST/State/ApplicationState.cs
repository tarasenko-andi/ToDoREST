using System;
using System.Collections.Immutable;
using System.Text;

namespace ToDoREST.State
{
    public class ApplicationState
    {
        public ImmutableArray<TodoItem> Todos { get; set; }

        public ApplicationState()
        {
            Todos = ImmutableArray<TodoItem>.Empty;
        }
    }
}
