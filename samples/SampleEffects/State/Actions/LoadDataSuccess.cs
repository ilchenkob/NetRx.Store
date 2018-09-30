using SampleEffects.State.Reducers;

namespace SampleEffects.State.Actions
{
    public class LoadDataSuccess : NetRx.Store.Action<Data>
    {
        public LoadDataSuccess(Data payload) : base(payload)
        {
        }
    }
}
