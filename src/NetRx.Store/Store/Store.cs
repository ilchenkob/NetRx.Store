using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Subjects;
using NetRx.Store.Exceptions;
using NetRx.Effects;
using Microsoft.CSharp.RuntimeBinder;

namespace NetRx.Store
{
    public sealed class Store : BlankStore
    {
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
        public new Store WithState<TState>(TState initialState, Reducer<TState> reducer)
        {
            try
            {
                return new Store
                {
                    _items = this._items
                                 .Concat(new List<StoreItem> { new StoreItem(new StateWrapper(initialState), reducer) })
                                 .ToList(),
                    _subscriptions = new ConcurrentDictionary<string, dynamic>(this._subscriptions),
                    _effectsWithoutResult = new Dictionary<string, IList<object>>(this._effectsWithoutResult),
                    _effectsWithResult = new Dictionary<string, IList<object>>(this._effectsWithResult)
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

        /// <summary>
        /// Adds effects to the store.
        /// </summary>
        /// <returns>Store that inludes passed effects</returns>
        /// <param name="effects">Effects collection</param>
        public Store WithEffects(IEnumerable<Effect> effects)
        {
            var storeEffectsWithoutResult = new Dictionary<string, IList<object>>(this._effectsWithoutResult);
            var storeEffectsWithResult = new Dictionary<string, IList<object>>(this._effectsWithResult);

            foreach (var effect in effects)
            {
                var typeOfInputAction = effect.GetType().BaseType.GenericTypeArguments[0].FullName;
                if (effect.GetType().BaseType.GenericTypeArguments.Length == 1)
                {
                    if (storeEffectsWithoutResult.ContainsKey(typeOfInputAction))
                        storeEffectsWithoutResult[typeOfInputAction].Add(effect as Effect<Action>);
                    else
                        storeEffectsWithoutResult.Add(typeOfInputAction, new List<object> { effect });
                }
                else if (effect.GetType().BaseType.GenericTypeArguments.Length == 2)
                {
                    if (storeEffectsWithResult.ContainsKey(typeOfInputAction))
                        storeEffectsWithResult[typeOfInputAction].Add(effect as Effect<Action, Action>);
                    else
                        storeEffectsWithResult.Add(typeOfInputAction, new List<object> { effect });
                }
            }

            return new Store
            {
                _items = this._items.ToList(),
                _subscriptions = new ConcurrentDictionary<string, dynamic>(this._subscriptions),
                _effectsWithoutResult = storeEffectsWithoutResult,
                _effectsWithResult = storeEffectsWithResult
            };
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
                _subscriptions = new ConcurrentDictionary<string, dynamic>(this._subscriptions),
                _effectsWithoutResult = new Dictionary<string, IList<object>>(),
                _effectsWithResult = new Dictionary<string, IList<object>>()
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

            TStateProperty lastValue = (TStateProperty)_items.FirstOrDefault(i => i.State.HasGeter(subscriptionName))
                                     .State.Get(subscriptionName);
            var subject = new BehaviorSubject<TStateProperty>(lastValue);
            _subscriptions.TryAdd(subscriptionName, subject);
            _subscriptions.TryGetValue(subscriptionName, out dynamic observable);
            return observable as IObservable<TStateProperty>;
        }

        /// <summary>
        /// Dispatches the action.
        /// </summary>
        /// <param name="action">Action to dispatch</param>
        public void Dispatch(Action action) => DispatchInternal(action);

        private void DispatchInternal(dynamic action)
        {
            var _prevStates = _items.Select(i => i.State).ToList();

            var modifiedStates = new List<(string stateName, StateWrapper pervState)>();
            foreach (var item in _items)
            {
                var prevValue = item.State;
                try
                {
                    var newValue = item.Reducer.Reduce((dynamic)item.State.Original, action);
                    item.State = new StateWrapper(newValue);
                    modifiedStates.Add((stateName: item.State.OriginalTypeName, prevValue));
                }
                catch (RuntimeBinderException)
                {
                    // skip this reducer, it doesn't support that action
                }
            }

            var actionTypeName = action.GetType().FullName;
            if (this._effectsWithoutResult.ContainsKey(actionTypeName))
                DispatchEffects(this._effectsWithoutResult[actionTypeName], action, false);
            if (this._effectsWithResult.ContainsKey(actionTypeName))
                DispatchEffects(this._effectsWithResult[actionTypeName], action, true);

            if (modifiedStates.Count > 0)
                DetectChanges(modifiedStates);
        }

        private void DispatchEffects(IList<object> effects, object action, bool triggersResultAction)
        {
            async void Invoke(dynamic effect, dynamic actionToInvoke)
            {
                if (triggersResultAction)
                    this.DispatchInternal(await effect.Invoke(actionToInvoke));
                else
                    effect.Invoke(actionToInvoke);
            }

            foreach (var effect in effects) Invoke(effect, action);
        }

        private void DetectChanges(List<(string stateName, StateWrapper pervState)> modifiedStates)
        {
            foreach(var changeInfo in modifiedStates)
            {
                var currState = _items.First(s => s.State.OriginalTypeName == changeInfo.stateName).State;
                var prevState = changeInfo.pervState;

                foreach (var field in currState.FieldNames)
                {
                    if (currState.HasGeter(field))
                    {
                        var newValue = currState.Get(field);
                        var prevValue = prevState.Get(field);

                        var hasChanges = field.EndsWith(StateWrapper.EnumerableFieldMarker, StringComparison.InvariantCulture)
                                              ? !Equals(newValue?.GetHashCode(), prevValue?.GetHashCode())
                                              : !Equals(newValue, prevValue);
                        if (hasChanges)
                        {
                            NotifySubscribers(currState, field.Replace(StateWrapper.EnumerableFieldMarker, string.Empty));
                        }
                    }
                }

                NotifySubscribers(currState, changeInfo.stateName);
            }
        }

        private void NotifySubscribers(StateWrapper wrappedState, string field)
        {
            if (_subscriptions.ContainsKey(field))
            {
                _subscriptions[field].OnNext((dynamic)wrappedState.Get(field));
            }
        }
    }
}
