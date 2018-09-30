using System;

namespace SampleEffects.State.Actions
{
    public class SetIsLoading : NetRx.Store.Action<bool>
    {
        public SetIsLoading(bool payload) : base(payload)
        {
        }
    }
}
