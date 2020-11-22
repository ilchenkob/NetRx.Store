using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using NetRx.Store.Diagnostic;
using NetRx.Store.Effects;

namespace NetRx.Store
{
    public sealed class Store : BlankStore, IStore
    {
        private readonly ITraceMessageWriter _messageWriter = new TraceMessageWriter();
        
        internal Store()
        {
        }

        /// <summary>
        /// Creates new store instance.
        /// </summary>
        /// <returns>Blank store instance</returns>
        public static BlankStore Create() => new BlankStore();

        /// <summary>
        /// Adds state to the store.
        /// </summary>
        /// <returns>Store that inludes passed state and reducer</returns>
        /// <param name="initialState">Initial state value</param>
        /// <param name="reducer">Reducer for the state</param>
        /// <typeparam name="TState">State type</typeparam>
        public new Store WithState<TState, TReducer>(TState initialState, TReducer reducer) where TReducer : Reducer<TState>
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
        public new Store WithState<TState>(TState initialState, Func<TState, Action, TState> reducer)
        {
            return TryCreateStore(() => WithState(
                new StoreItem<TState>(
                        StateWrapper.ForObject(() => initialState),
                        reducer)
                )
            );
        }

        /// <summary>
        /// Adds effects to the store.
        /// </summary>
        /// <returns>Store that inludes passed effects</returns>
        /// <param name="effects">Effects collection</param>
        public Store WithEffects(IEnumerable<Effect> effects)
        {
            var storeEffects = new Dictionary<string, IList<IEffectMethodWrapper>>(this._effects);
            var store = new Store
            {
                _items = this._items.ToList(),
                _subscriptions = new ConcurrentDictionary<string, ISubscription>(this._subscriptions),
                _effects = storeEffects
            };

            foreach (var effect in effects)
            {
                var wrapper = EffectWrapper.FromObject(store, effect);
                if (storeEffects.ContainsKey(wrapper.ActionTypeName))
                    storeEffects[wrapper.ActionTypeName].Add(wrapper);
                else
                    storeEffects.Add(wrapper.ActionTypeName, new List<IEffectMethodWrapper> { wrapper });
            }

            return store;
        }

        /// <summary>
        /// Removes effects from the store.
        /// </summary>
        /// <returns>Store that have no effects</returns>
        public Store WithoutEffects()
        {
            return new Store
            {
                _items = this._items.ToList(),
                _subscriptions = new ConcurrentDictionary<string, ISubscription>(this._subscriptions),
                _effects = new Dictionary<string, IList<IEffectMethodWrapper>>()
            };
        }

        /// <summary>
        /// Selects the specified state property.
        /// </summary>
        /// <returns>Observable for the specified state property</returns>
        /// <param name="propertyExpression">Property expression</param>
        /// <typeparam name="TState">State type</typeparam>
        /// <typeparam name="TStateProperty">Target property type</typeparam>
        public IObservable<TStateProperty> Select<TState, TStateProperty>(Expression<Func<TState, TStateProperty>> propertyExpression)
        {
            var memberName = string.Empty;
            if (propertyExpression.Body is MemberExpression memberExpr)
            {
                memberName = memberExpr.ToString();
                memberName = memberName.Substring(memberName.IndexOf('.'));
            }

            var item = _items.FirstOrDefault(s => s.State.Original is TState);
            if (item == null)
            {
                throw new InvalidOperationException("State of such type not found");
            }

            var subscriptionName = $"{typeof(TState).FullName}{memberName}";
            var subscription = _subscriptions.GetOrAdd(subscriptionName, (propName) =>
            {
                TStateProperty lastValue = (TStateProperty)_items.FirstOrDefault(i => i.State.HasGeter(subscriptionName))
                                                                 .State.Get(subscriptionName);
                return new Subscription<TStateProperty>(lastValue);
            });

            return ((Subscription<TStateProperty>)subscription).AsObservable();
        }

        /// <summary>
        /// Dispatches the action.
        /// </summary>
        /// <param name="action">Action to dispatch</param>
        public void Dispatch<T>(T action) where T : Action
        {
            var actionTypeName = typeof(T).FullName;

            var modifiedStates = new List<(string, StateWrapper)>();
            foreach (var item in _items)
            {
                var prevValue = item.State;
                var newValue = item.Dispatch(action, actionTypeName);
                if (newValue != null)
                {
                    item.State = new StateWrapper(newValue, item.State.OriginalTypeName);
                    modifiedStates.Add((item.State.OriginalTypeName, prevValue));
                }
            }

            if (modifiedStates.Count > 0)
                DetectChanges(modifiedStates);

            if (Debugger.IsAttached)
            {
                _messageWriter.Write(actionTypeName, _items);
            }

            if (this._effects.ContainsKey(actionTypeName))
                DispatchEffects(this._effects[actionTypeName], action);
        }

        private void DispatchEffects<T>(IList<IEffectMethodWrapper> effects, T action) where T : Action
        {
            foreach (var effect in effects) effect.Invoke(action);
        }

        private void DetectChanges(List<(string, StateWrapper)> modifiedStates)
        {
            foreach(var changeInfo in modifiedStates)
            {
                var currState = _items.First(s => s.State.OriginalTypeName == changeInfo.Item1).State;
                var prevState = changeInfo.Item2;

                foreach (var field in currState.FieldNames)
                {
                    if (currState.HasGeter(field))
                    {
                        var newValue = currState.Get(field);
                        var prevValue = prevState.Get(field);

                        var hasChanges = field.EndsWith(StateWrapper.EnumerableFieldMarker, StringComparison.InvariantCulture)
                                              ? !Equals(newValue?.GetHashCode(), prevValue?.GetHashCode())
                                              : field.EndsWith(StateWrapper.ReferenceFieldMarker, StringComparison.InvariantCulture)
                                                ? !ReferenceEquals(newValue,prevValue)
                                                : !Equals(newValue, prevValue);
                        if (hasChanges)
                        {
                            NotifySubscribers(currState, 
                                field.Replace(StateWrapper.EnumerableFieldMarker, string.Empty)
                                .Replace(StateWrapper.ReferenceFieldMarker, string.Empty));
                        }
                    }
                }

                NotifySubscribers(currState, changeInfo.Item1);
            }
        }

        private void NotifySubscribers(StateWrapper wrappedState, string field)
        {
            if (_subscriptions.ContainsKey(field))
            {
                _subscriptions[field].OnNext(wrappedState.Get(field));
            }
        }

        private Store WithState(StoreItem item)
        {
            return new Store
            {
                _items = this._items.Concat(new List<StoreItem> { item }).ToList(),
                _subscriptions = new ConcurrentDictionary<string, ISubscription>(this._subscriptions),
                _effects = new Dictionary<string, IList<IEffectMethodWrapper>>(this._effects)
            };
        }
    }
}
