using System.Threading.Tasks;
using SampleMVVM.Wpf.Models.Services;
using NetRx.Effects;

namespace SampleMVVM.Wpf.Models.State.Effects
{
  public class LoadDataEffect : Effect<DataActions.LoadDataStart, NetRx.Store.Action>
  {
    private readonly IDataService _dataService;

    public LoadDataEffect(IDataService dataService)
    {
      _dataService = dataService;
    }

    public override async Task<NetRx.Store.Action> Invoke(DataActions.LoadDataStart action)
    {
      try
      {
        var result = await _dataService.GetDataItems(action.Payload);
        return new DataActions.LoadDataSuccess(result);
      }
      catch
      {
        return new DataActions.LoadDataFailed();
      }
    }
  }

  public class SendItemEffect : Effect<DataActions.SendItemStart, NetRx.Store.Action>
  {
    private readonly IDataService _dataService;

    public SendItemEffect(IDataService dataService)
    {
      _dataService = dataService;
    }

    public override async Task<NetRx.Store.Action> Invoke(DataActions.SendItemStart action)
    {
      try
      {
        var result = await _dataService.SendItem(action.Payload);
        return new DataActions.SendItemSuccess(action.Payload);
      }
      catch
      {
        return new DataActions.SendItemFailed();
      }
    }
  }

  public static class DataEffects
  {
    public static Effect[] GetAll(IDataService dataService)
    {
      return new Effect[]
      {
        new LoadDataEffect(dataService),
        new SendItemEffect(dataService),
      };
    }
  }
}
