using System;
using System.Collections.Generic;
using System.Text;
using ToDoREST.Actions;

namespace ToDoREST.State
{
    public interface IStore<TState> : IObservable<TState>
    {
        IAction Dispatch(IAction action);
        TState GetState();
    }
}
