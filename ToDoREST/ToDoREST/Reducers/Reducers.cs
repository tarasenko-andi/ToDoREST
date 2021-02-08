using System;
using System.Collections.Generic;
using System.Text;
using ToDoREST.Models;

namespace ToDoREST.Reducers
{
	public class Reducers
	{
		public static ToDoREST.State.ApplicationState ReduceApplication(ToDoREST.State.ApplicationState previousState, Actions.IAction action)
		{
			return new ToDoREST.State.ApplicationState
			{
				Todos = TodoReducers.Reduce(previousState.Todos, action)
			};
		}
	}
}
