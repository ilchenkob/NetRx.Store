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

  public class SendItemResult : NetRx.Store.Action<DataItem>
  {
    public SendItemResult(DataItem payload) : base(payload)
    {
    }
  }

  public class LoadDataStart : NetRx.Store.Action<int>
  {
    public LoadDataStart(int payload) : base(payload)
    {
    }
  }

  public class LoadDataResult : NetRx.Store.Action<List<DataItem>>
  {
    public LoadDataResult(List<DataItem> payload) : base(payload)
    {
    }
  }
}
