using System.Collections.Concurrent;
using System.Collections.Generic;
using NetRx.Effects;
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
            try
            {
                return new Store
                {
                    _items = new List<StoreItem>
                    {
                        new StoreItem(
                            StateWrapper.ForObject(() => initialState),
                            ReducerWrapper.ForObject<TState, TReducer>(reducer)
                        )
                    },
                    _subscriptions = new ConcurrentDictionary<string, ISubscription>(),
                    _effects = new Dictionary<string, IList<IEffectMethodWrapper>>()
                };
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
    }
}
