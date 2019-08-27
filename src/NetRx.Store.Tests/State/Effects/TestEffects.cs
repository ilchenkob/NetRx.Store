﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetRx.Store.Effects;

namespace NetRx.Store.Tests.State.Effects
{
    public class SetItemsEffect : Effect<TestStateActions.SetItemsAction>
    {
        public int CallCount { get; private set; }

        public override Task Invoke(TestStateActions.SetItemsAction action)
        {
            CallCount++;
            return Task.CompletedTask;
        }
    }

    public class LoadItemsEffect : Effect<TestStateActions.LoadItemsAction, TestStateActions.SetItemsAction>
    {
        private readonly List<string> _result;

        public int CallCount { get; private set; }

        public LoadItemsEffect(IEnumerable<string> result)
        {
            _result = result.ToList();
        }

        public LoadItemsEffect() : this(new List<string>())
        {
        }

        public override Task<TestStateActions.SetItemsAction> Invoke(TestStateActions.LoadItemsAction action)
        {
            CallCount++;
            return Task.FromResult(new TestStateActions.SetItemsAction(_result));
        }
    }

    public class LogItemsLoadingEffect : Effect<TestStateActions.LoadItemsAction>
    {
        private readonly List<string> _result;

        public int CallCount { get; private set; }

        public LogItemsLoadingEffect(IEnumerable<string> result)
        {
            _result = result.ToList();
        }

        public LogItemsLoadingEffect() : this(new List<string>())
        {
        }

        public override Task Invoke(TestStateActions.LoadItemsAction action)
        {
            CallCount++;
            return Task.CompletedTask;
        }
    }
}
