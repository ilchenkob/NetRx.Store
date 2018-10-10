using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NetRx.Store.Tests.State.Reducers
{
    public class TestStateReducer : Reducer<TestState>
    {
        public TestState Reduce(TestState state, TestStateActions.ClearNameAction action)
        {
            state.Name = string.Empty;
            return state;
        }

        public TestState Reduce(TestState state, TestStateActions.SetItemsAction action)
        {
            state.Items = action.Payload.ToImmutableList();
            return state;
        }
    }

    public class SecondaryTestStateReducer : Reducer<SecondaryTestState>
    {
    }
}
