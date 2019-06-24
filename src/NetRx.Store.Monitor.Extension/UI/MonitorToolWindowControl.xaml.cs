namespace NetRx.Store.Monitor.Extension.UI
{
    using NetRx.Store.Monitor.Extension.Logic.ViewModels;
    using System.Windows.Controls;

    public partial class MonitorToolWindowControl : UserControl
    {
        public MonitorToolWindowControl(MonitorToolViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;
        }
    }
}