using System.Threading.Tasks;
using SampleMVVM.Wpf.Models.State.DataActions;
using NetRx.Effects;

namespace SampleMVVM.Wpf.Models.State.Effects
{
  public class SendingFailedMessageEffect : NetRx.Effects.Effect<DataActions.SendItemFailed>
  {
    public override Task Invoke(SendItemFailed action)
    {
      return App.ShowMessge("Data item sending failed", true);
    }
  }

  public class DataLoadingFailedMessageEffect : NetRx.Effects.Effect<DataActions.LoadDataFailed>
  {
    public override Task Invoke(LoadDataFailed action)
    {
      return App.ShowMessge("Loading failed", true);
    }
  }

  public class DataLoadingSuccessMessageEffect : NetRx.Effects.Effect<DataActions.LoadDataSuccess>
  {
    public override Task Invoke(LoadDataSuccess action)
    {
      return App.ShowMessge($"Loaded {action.Payload.Count} items");
    }
  }

  public static class MessageEffects
  {
    public static Effect[] GetAll()
    {
      return new Effect[]
      {
        new SendingFailedMessageEffect(),
        new DataLoadingFailedMessageEffect(),
        new DataLoadingSuccessMessageEffect(),
      };
    }
  }
}
