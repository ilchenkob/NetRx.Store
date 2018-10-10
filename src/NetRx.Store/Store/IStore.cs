using System;
using System.Linq.Expressions;

namespace NetRx.Store
{
    public interface IStore
    {
        void Dispatch<T>(T action) where T : Action;

        IObservable<TStateProperty> Select<TState, TStateProperty>(Expression<Func<TState, TStateProperty>> propertyExpression);
    }
}