using System.Threading.Tasks;

namespace NetRx.Store.Effects
{
    public abstract class Effect
    {
        internal const string InvokeMethodName = "Invoke";
    }

    public abstract class Effect<TInputAction> : Effect where TInputAction : Action
    {
        public abstract Task Invoke(TInputAction action);
    }

    public abstract class Effect<TInputAction, TOutputAction> : Effect
        where TInputAction : Action
        where TOutputAction : Action
    {
        public abstract Task<TOutputAction> Invoke(TInputAction action);
    }
}
