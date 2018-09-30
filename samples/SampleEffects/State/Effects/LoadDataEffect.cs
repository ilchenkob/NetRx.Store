using System;
using System.Threading.Tasks;
using NetRx.Effects;
using SampleEffects.State.Reducers;
using actions = SampleEffects.State.Actions;

namespace SampleEffects.State.Effects
{
    public class LoadDataEffect : Effect<actions.LoadData, actions.LoadDataSuccess>
    {
        public override async Task<actions.LoadDataSuccess> Invoke(actions.LoadData action)
        {
            await Task.Delay(1500);
            return new actions.LoadDataSuccess(new Data
            {
                Count = DateTime.Now.Second,
                Amount = DateTime.Now.Minute,
                Category = Guid.NewGuid().ToString()
            });
        }
    }
}
