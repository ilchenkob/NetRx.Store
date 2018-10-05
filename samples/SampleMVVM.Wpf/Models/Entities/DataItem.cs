using System;

namespace SampleMVVM.Wpf.Models.Entities
{
  public struct DataItem
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; }
  }
}
