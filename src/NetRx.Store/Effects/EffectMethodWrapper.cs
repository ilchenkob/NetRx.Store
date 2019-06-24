using System;
using System.Threading.Tasks;

namespace NetRx.Store.Effects
{
    internal interface IEffectMethodWrapper
    {
        string ActionTypeName { get; }

        Task Invoke(Action action);
    }

    internal class EffectMethodWrapper<T> : IEffectMethodWrapper where T : Action
    {
        private readonly Func<T, Task> _func;

        public EffectMethodWrapper(string actionTypeName, Delegate del)
        {
            ActionTypeName = actionTypeName;
            _func = (Func<T, Task>)del;
        }

        public string ActionTypeName { get; private set; }

        public Task Invoke(Action action) => _func((T)action);
    }

    internal class EffectMethodWrapper<TInputAction, TOutputAction>
        : IEffectMethodWrapper
            where TInputAction : Action
            where TOutputAction : Action
    {
        private readonly Func<TInputAction, Task<TOutputAction>> _func;
        private readonly Store _store;

        public EffectMethodWrapper(string actionTypeName, Delegate del, NetRx.Store.Store store)
        {
            ActionTypeName = actionTypeName;
            _func = (Func<TInputAction, Task<TOutputAction>>)del;
            _store = store;
        }

        public string ActionTypeName { get; private set; }

        public async Task Invoke(Action action)
        {
            _store.Dispatch(await _func((TInputAction)action));
        }
    }
}
