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
        private EnvDTE.Events _ideEvents;
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

            _ideEvents = MonitorToolWindowCommand.Instance.Ide.Events;
            _ideEvents.DebuggerEvents.OnEnterDesignMode += OnExitDebuggerMode;

            _debugPaneListener = new DebugPaneListener(_ideEvents.OutputWindowEvents, new OutputPaneParser());
            _debugPaneListener.HandleUpdate = message => _monitorToolViewModel.AddStateRecord(message);

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new MonitorToolWindowControl(_monitorToolViewModel);
        }

        private void OnExitDebuggerMode(EnvDTE.dbgEventReason Reason)
        {
            _monitorToolViewModel?.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            _debugPaneListener.Dispose();

            ThreadHelper.ThrowIfNotOnUIThread();
            _ideEvents.DebuggerEvents.OnEnterDesignMode -= OnExitDebuggerMode;

            base.Dispose(disposing);
        }
    }
}
