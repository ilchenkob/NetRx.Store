using System.Collections.Concurrent;
using System.Collections.Generic;
using NetRx.Store.Exceptions;

namespace NetRx.Store
{
    public class BlankStore
    {
        internal IList<StoreItem> _items;

        internal ConcurrentDictionary<string, dynamic> _subscriptions;

        internal Dictionary<string, IList<object>> _effectsWithoutResult;

        internal Dictionary<string, IList<object>> _effectsWithResult;

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
        public Store WithState<TState>(TState initialState, Reducer<TState> reducer)
        {
            try
            {
                return new Store
                {
                    _items = new List<StoreItem> { new StoreItem(StateWrapper.ForObject(() => initialState), reducer) },
                    _subscriptions = new ConcurrentDictionary<string, dynamic>(),
                    _effectsWithoutResult = new Dictionary<string, IList<object>>(),
                    _effectsWithResult = new Dictionary<string, IList<object>>()
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
