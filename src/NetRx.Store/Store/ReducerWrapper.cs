using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetRx.Store
{
    internal class ReducerWrapper<TState> : ReducerWrapper
    {
        private readonly Dictionary<string, IReduceMethod> _actionHandlers;

        internal ReducerWrapper(Dictionary<string, IReduceMethod> actionHandlers)
        {
            _actionHandlers = actionHandlers;
        }

        public override object Invoke(string actionTypeFullName, object state, object action)
                    => _actionHandlers[actionTypeFullName].Invoke(state, action);

        public override bool CanHandle(string actionTypeFullName)
                    => _actionHandlers.ContainsKey(actionTypeFullName);
    }

    internal abstract class ReducerWrapper
    {
        private const string ReducerMethodName = "Reduce";

        internal static ReducerWrapper ForObject<T, TReducer>(object reducer)
        {
            Type baseActionType = typeof(Action);
            Type reducerType = typeof(TReducer);
            Type stateType = typeof(T);

            var reducerMethods = reducerType.GetMethods()
            .Where(m => // filter all methods that has the following syntax: TState Reduce(TState state, TAction action)
            {
                var parameters = m.GetParameters();
                return string.Equals(m.Name, ReducerMethodName) &&
                                    m.ReturnType == stateType &&
                                    parameters.Length == 2 &&
                                    parameters[0].ParameterType == stateType &&
                                    isNestedFromType(parameters[1].ParameterType, baseActionType);
            })
            .Select(m => // create Func-s from method information
            {
                var methodParams = m.GetParameters();
                var actionType = methodParams[1].ParameterType;
                var stateParam = Expression.Parameter(methodParams[0].ParameterType, methodParams[0].Name);
                var actionParam = Expression.Parameter(actionType, methodParams[1].Name);

                var methodExpr = Expression.Lambda(
                    Expression.Call(Expression.Constant(reducer, reducerType), m, stateParam, actionParam),
                    stateParam,
                    actionParam
                );

                var reduceFunc = methodExpr.Compile();

                var reduceMethodConstructor = typeof(ReduceMethod<,>)
                                            .MakeGenericType(new Type[] { stateType, actionType })
                                            .GetConstructors()[0];
                return new
                {
                    ActionName = m.GetParameters()[1].ParameterType.FullName,
                    ReduceMethod = (IReduceMethod)reduceMethodConstructor.Invoke(new[] { reduceFunc })
                };
            })
            .ToDictionary(k => k.ActionName, v => v.ReduceMethod);

            return new ReducerWrapper<T>(reducerMethods);
        }

        public abstract object Invoke(string actionTypeFullName, object state, object action);

        public abstract bool CanHandle(string actionTypeFullName);

        private static bool isNestedFromType(Type type, Type expectedType)
        {
            var typeToCheck = type;
            while(typeToCheck != null)
            {
                if (typeToCheck == expectedType)
                return true;
                else
                typeToCheck = typeToCheck.BaseType;
            }
            return false;
        }

        internal interface IReduceMethod
        {
            object Invoke(object state, object action);
        }

        internal class ReduceMethod<TState, TAction> : IReduceMethod
        {
            private readonly Func<TState, TAction, TState> _func;

            public ReduceMethod(Delegate del)
            {
                _func = (Func<TState, TAction, TState>)del;
            }

            public object Invoke(object state, object action) => _func((TState)state, (TAction)action);
        }
    }
}