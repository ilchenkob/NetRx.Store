using SampleMVVM.Wpf.Models.Entities;
using System.Collections.Generic;

namespace SampleMVVM.Wpf.Models.State.DataActions
{
  public class SendItemStart : NetRx.Store.Action<DataItem>
  {
    public SendItemStart(DataItem payload) : base(payload)
    {
    }
  }

  public class SendItemSuccess : NetRx.Store.Action<DataItem>
  {
    public SendItemSuccess(DataItem payload) : base(payload)
    {
    }
  }

  public class SendItemFailed : NetRx.Store.Action
  {
  }

  public class LoadDataStart : NetRx.Store.Action<int>
  {
    public LoadDataStart(int payload) : base(payload)
    {
    }
  }

  public class LoadDataSuccess : NetRx.Store.Action<List<DataItem>>
  {
    public LoadDataSuccess(List<DataItem> payload) : base(payload)
    {
    }
  }

  public class LoadDataFailed : NetRx.Store.Action
  {
  }
}
