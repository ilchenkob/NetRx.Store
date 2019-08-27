using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NetRx.Store.Diagnostic;
using NetRx.Store.Effects;
using NetRx.Store.Exceptions;

namespace NetRx.Store
{
    public class BlankStore
    {
        internal IList<StoreItem> _items;

        internal ConcurrentDictionary<string, ISubscription> _subscriptions;

        internal Dictionary<string, IList<IEffectMethodWrapper>> _effects;

        internal BlankStore()
        {
        }

        /// <summary>
        /// Adds state to the store
        /// </summary>
        /// <returns>Store that inludes passed state and reducer</returns>
        /// <param name="initialState">Initial state value</param>
        /// <param name="reducer">Reducer for the state</param>
        /// <typeparam name="TState">State type</typeparam>
        public Store WithState<TState, TReducer>(TState initialState, TReducer reducer) where TReducer : Reducer<TState>
        {
            return TryCreateStore(() => WithState(
                new StoreItem(
                        StateWrapper.ForObject(() => initialState),
                        ReducerWrapper.ForObject<TState, TReducer>(reducer))
                )
            );
        }

        /// <summary>
        /// Adds state to the store
        /// </summary>
        /// <returns>Store that inludes passed state and reducer</returns>
        /// <param name="initialState">Initial state value</param>
        /// <param name="reducer">Reducer function</param>
        /// <typeparam name="TState">State type</typeparam>
        public Store WithState<TState>(TState initialState, Func<TState, Action, TState> reducer)
        {
            return TryCreateStore(() => WithState(
                new StoreItem<TState>(
                        StateWrapper.ForObject(() => initialState),
                        reducer)
                )
            );
        }

        protected Store TryCreateStore(Func<Store> createFunc)
        {
            try
            {
                return createFunc();
            }
            catch (InvalidStateTypeException stateTypeException)
            {
                throw new InvalidStateTypeException(stateTypeException.Message);
            }
            catch (InvalidStatePropertyTypeException propTypeException)
            {
                throw new InvalidStatePropertyTypeException(propTypeException.Message);
            }
        }

        private Store WithState(StoreItem item)
        {
            return TryCreateStore(() => new Store
            {
                _items = new List<StoreItem> { item },
                _subscriptions = new ConcurrentDictionary<string, ISubscription>(),
                _effects = new Dictionary<string, IList<IEffectMethodWrapper>>()
            });
        }
    }
}
