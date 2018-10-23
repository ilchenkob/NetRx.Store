using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Action = NetRx.Store.Action;

namespace NetRx.Effects
{
    internal abstract class EffectWrapper
    {
        public static IEffectMethodWrapper FromObject(Store.Store store, Effect effect)
        {
            var effectType = effect.GetType();
            var effectTypeGenericArguments = effectType.BaseType.GenericTypeArguments;
            var typeOfInputAction = effectTypeGenericArguments[0].FullName;

            var invokeMethod = effectType.GetMethod(Effect.InvokeMethodName);
            var actionArg = invokeMethod.GetParameters()[0];
            var actionParameter = Expression.Parameter(actionArg.ParameterType, actionArg.Name);

            var methodExpr = Expression.Lambda(
                Expression.Call(Expression.Constant(effect, effectType), invokeMethod, actionParameter),
                actionParameter
            );

            if (effectTypeGenericArguments.Length == 1)
            {
                return (IEffectMethodWrapper)typeof(EffectMethodWrapper<>)
                .MakeGenericType(new Type[]
                {
                    actionArg.ParameterType
                })
                .GetConstructors()[0]
                .Invoke(new object[]
                {
                    typeOfInputAction,
                    methodExpr.Compile()
                });
            }
            else if (effectTypeGenericArguments.Length == 2)
            {
                return (IEffectMethodWrapper)typeof(EffectMethodWrapper<,>)
                .MakeGenericType(new Type[]
                {
                    actionArg.ParameterType,
                    invokeMethod.ReturnType.GenericTypeArguments[0]
                })
                .GetConstructors()[0]
                .Invoke(new object[]
                {
                    typeOfInputAction,
                    methodExpr.Compile(),
                    store
                });
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
