using System.Threading.Tasks;
using SampleMVVM.Wpf.Models.Services;
using NetRx.Effects;
using System.Collections.Generic;
using SampleMVVM.Wpf.Models.Entities;

namespace SampleMVVM.Wpf.Models.State.Effects
{
  public class LoadDataEffect : Effect<DataActions.LoadDataStart, DataActions.LoadDataResult>
  {
    private readonly IDataService _dataService;

    public LoadDataEffect(IDataService dataService)
    {
      _dataService = dataService;
    }

    public override async Task<DataActions.LoadDataResult> Invoke(DataActions.LoadDataStart action)
    {
      try
      {
        var result = await _dataService.GetDataItems(action.Payload);
        return new DataActions.LoadDataResult(result);
      }
      catch
      {
        return new DataActions.LoadDataResult(new List<DataItem>());
      }
    }
  }

  public class SendItemEffect : Effect<DataActions.SendItemStart, DataActions.SendItemResult>
  {
    private readonly IDataService _dataService;

    public SendItemEffect(IDataService dataService)
    {
      _dataService = dataService;
    }

    public override async Task<DataActions.SendItemResult> Invoke(DataActions.SendItemStart action)
    {
      try
      {
        var result = await _dataService.SendItem(action.Payload);
        return new DataActions.SendItemResult(action.Payload);
      }
      catch
      {
        return new DataActions.SendItemResult(new DataItem { Id = -1 });
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
