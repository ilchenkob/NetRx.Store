namespace NetRx.Store.Monitor.Extension
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using NetRx.Store.Monitor.Extension.Logic.ViewModels;
    using NetRx.Store.Monitor.Extension.UI;
    using NetRx.Store.Monitor.Logic;
    
    [Guid("680bdfa0-4831-42aa-82a5-931619a034e5")]
    public class MonitorToolWindow : ToolWindowPane
    {
        private EnvDTE.DebuggerEvents _debuggerEvents;
        private DebugPaneListener _debugPaneListener;
        private MonitorToolViewModel _monitorToolViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorToolWindow"/> class.
        /// </summary>
        public MonitorToolWindow() : base(null)
        {
            this.Caption = "NetRx.Store Monitor";

            _monitorToolViewModel = new MonitorToolViewModel();

            ThreadHelper.ThrowIfNotOnUIThread();

            _debuggerEvents = MonitorToolWindowCommand.Instance.Ide.Events.DebuggerEvents;
            _debuggerEvents.OnEnterDesignMode += OnExitDebuggerMode;

            _debugPaneListener = new DebugPaneListener(
                MonitorToolWindowCommand.Instance.Ide.Events.OutputWindowEvents,
                new OutputPaneParser())
            {
                HandleUpdate = message => _monitorToolViewModel.AddStateRecord(message)
            };

            this.Content = new MonitorToolWindowControl(_monitorToolViewModel);
        }

        private void OnExitDebuggerMode(EnvDTE.dbgEventReason Reason)
        {
            _monitorToolViewModel?.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            _debugPaneListener.Dispose();
            _debuggerEvents.OnEnterDesignMode -= OnExitDebuggerMode;

            base.Dispose(disposing);
        }
    }
}
