using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleMVVM.Wpf.Models.Entities;

namespace SampleMVVM.Wpf.Models.Services
{
  public class DataService : IDataService
  {
    public async Task<List<DataItem>> GetDataItems(int userId)
    {
      var now = DateTime.Now;

      // simulate the waiting for the response
      await Task.Delay(TimeSpan.FromSeconds(1));

      return Enumerable.Range(0, now.Second)
        .Select(i => new DataItem
        {
          Id = i,
          Amount = now.Millisecond + i * 10,
          CreatedAt = now,
          Title = $"{now.Second} - {i}",
          IsPrimary = i == 30
        })
        .ToList();
    }

    public async Task<bool> SendItem(DataItem item)
    {
      // simulate the waiting for the response
      await Task.Delay(TimeSpan.FromSeconds(1));

      return true;
    }
  }
}
