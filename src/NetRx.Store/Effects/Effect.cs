using System.Threading.Tasks;
using NetRx.Store;

namespace NetRx.Effects
{
    public abstract class Effect
    {
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
