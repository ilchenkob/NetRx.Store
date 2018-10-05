namespace SampleMVVM.Wpf.Models.State.States
{
  public struct UserState
  {
    public int UserId { get; set; }

    public string Login { get; set; }

    public bool IsLoading { get; set; }

    public static UserState Initial()
    {
      return new UserState
      {
        UserId = 0,
        Login = string.Empty,
        IsLoading = false
      };
    }
  }
}
