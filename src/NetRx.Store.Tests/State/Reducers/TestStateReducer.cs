using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NetRx.Store.Tests.State.Reducers
{
    public class TestStateReducer : Reducer<TestState>
    {
        public IList<string> ReduceCalls { get; private set; }  = new List<string>();

        public TestState Reduce(TestState state, TestStateActions.ClearNameAction action)
        {
            ReduceCalls.Add(action.GetType().FullName);
            state.Name = string.Empty;
            return state;
        }

        public TestState Reduce(TestState state, TestStateActions.SetItemsAction action)
        {
            ReduceCalls.Add(action.GetType().FullName);
            state.Items = action.Payload.ToImmutableList();
            return state;
        }
    }

    public class SecondaryTestStateReducer : Reducer<SecondaryTestState>
    {
    }
}
