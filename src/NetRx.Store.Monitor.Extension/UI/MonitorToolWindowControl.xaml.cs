namespace NetRx.Store.Monitor.Extension.UI
{
    using Microsoft.VisualStudio.Shell;
    using NetRx.Store.Monitor.Extension.Logic.ViewModels;
    using System;
    using System.ComponentModel;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public partial class MonitorToolWindowControl : UserControl
    {
        public MonitorToolWindowControl(MonitorToolViewModel viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;

            Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                    h => FilterTextBox.TextChanged += h,
                    h => FilterTextBox.TextChanged -= h)
                .Throttle(TimeSpan.FromMilliseconds(700))
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
                .Subscribe(async arg =>
                {
                    if (viewModel.SelectedStoreHistory != null && arg.Sender is TextBox textBox)
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        viewModel.SelectedStoreHistory.ActionFilter = textBox.Text.ToLower();
                        viewModel.SelectedStoreHistory.ApplyFilterCommand?.Execute(null);
                    }
                });
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
        }
    }
}