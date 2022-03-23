using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HuatuoLib
{
    public class TaskHelper
    {
        //public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        //{
        //    if (stateMachine == null)
        //    {
        //        throw new ArgumentNullException("stateMachine");
        //    }
        //    ExecutionContextSwitcher executionContextSwitcher = default(ExecutionContextSwitcher);
        //    RuntimeHelpers.PrepareConstrainedRegions();
        //    try
        //    {
        //        ExecutionContext.EstablishCopyOnWriteScope(ref executionContextSwitcher);
        //        stateMachine.MoveNext();
        //    }
        //    finally
        //    {
        //        executionContextSwitcher.Undo();
        //    }
        //}
    }
}
