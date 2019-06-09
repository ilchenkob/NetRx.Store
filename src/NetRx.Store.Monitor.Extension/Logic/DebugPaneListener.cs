using EnvDTE;
using System;
using System.Diagnostics;
using NetRx.Store.Monitor.Shared;
using MonitorConstants = NetRx.Store.Monitor.Shared.Constants;
using NetRx.Store.Monitor.Shared.Models;
using Microsoft.VisualStudio.Shell;

namespace NetRx.Store.Monitor.Logic
{
    public class DebugPaneListener : IDisposable
    {
        private const string TargetPaneName = "Debug";

        private readonly OutputWindowEvents _outputWindowEvents;
        private readonly IOutputPaneParser _parser;

        public DebugPaneListener(OutputWindowEvents outputWindowEvents, IOutputPaneParser paneParser)
        {
            Debug.Assert(outputWindowEvents != null);
            Debug.Assert(paneParser != null);

            _outputWindowEvents = outputWindowEvents;
            _outputWindowEvents.PaneUpdated += onPaneUpdated;

            _parser = paneParser;
        }

        public void Dispose()
        {
            _outputWindowEvents.PaneUpdated -= onPaneUpdated;
        }

        public Action<Message> HandleUpdate { get; set; }

        private void onPaneUpdated(OutputWindowPane pane)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pane.Name == TargetPaneName)
            {
                var lastLine = _parser.GetLastLine(pane.TextDocument);
                if (!string.IsNullOrWhiteSpace(lastLine) && lastLine.StartsWith(MonitorConstants.Message.Tag))
                {
                    HandleUpdate?.Invoke(MessageConverter.Deserialize(lastLine));
                }
            }
        }
    }
}
