using System;
using System.Threading.Tasks;
using NetRx.Effects;
using actions = SampleEffects.State.Actions;

namespace SampleEffects.State.Effects
{
    public class UsernameChangedEffect : Effect<actions.SetUsername>
    {
        public override async Task Invoke(actions.SetUsername action)
        {
            await Task.Delay(250);
            Console.WriteLine($"Effect. Username value: {action.Payload}");
        }
    }
}
