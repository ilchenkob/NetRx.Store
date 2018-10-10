using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NetRx.Effects;

namespace NetRx.Store.Tests.State.Effects
{
    public class SetItemsEffect : Effect<TestStateActions.SetItemsAction>
    {
        public override Task Invoke(TestStateActions.SetItemsAction action)
        {
            return Task.CompletedTask;
        }
    }

    public class LoadItemsEffect : Effect<TestStateActions.LoadItemsAction, TestStateActions.SetItemsAction>
    {
        private readonly List<string> _result;

        public LoadItemsEffect(IEnumerable<string> result)
        {
            _result = result.ToList();
        }

        public LoadItemsEffect() : this(new List<string>())
        {
        }

        public override Task<TestStateActions.SetItemsAction> Invoke(TestStateActions.LoadItemsAction action)
        {
            return Task.FromResult(new TestStateActions.SetItemsAction(_result));
        }
    }
}
