using System.Collections.Generic;
using System.Threading.Tasks;
using SampleMVVM.Wpf.Models.Entities;

namespace SampleMVVM.Wpf.Models.Services
{
  public interface IDataService
  {
    Task<List<DataItem>> GetDataItems(int userId);

    Task<bool> SendItem(DataItem item);
  }
}