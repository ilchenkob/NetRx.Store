using System.Threading.Tasks;
using SampleMVVM.Wpf.Models.State.DataActions;
using NetRx.Store.Effects;

namespace SampleMVVM.Wpf.Models.State.Effects
{
  public class SendingFailedMessageEffect : Effect<DataActions.SendItemResult>
  {
    public override Task Invoke(SendItemResult action)
    {
      return action.Payload.Id > 0
        ? App.ShowMessge("Data item has been sent")
        : App.ShowMessge("Data item sending failed", true);
    }
  }

  public class DataLoadingFailedMessageEffect : Effect<DataActions.LoadDataResult>
  {
    public override Task Invoke(LoadDataResult action)
    {
      return action.Payload.Count > 0
        ? App.ShowMessge($"Loaded {action.Payload.Count} items")
        : App.ShowMessge("Loading failed", true);
    }
  }

  public static class MessageEffects
  {
    public static Effect[] GetAll()
    {
      return new Effect[]
      {
        new SendingFailedMessageEffect(),
        new DataLoadingFailedMessageEffect()
      };
    }
  }
}
