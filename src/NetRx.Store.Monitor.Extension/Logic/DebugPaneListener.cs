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

        public Action<TraceMessage> HandleUpdate { get; set; }

        private void onPaneUpdated(OutputWindowPane pane)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pane.Name == TargetPaneName)
            {
                var lastLines = _parser.GetLastLines(pane.TextDocument);
                foreach (var line in lastLines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.StartsWith(MonitorConstants.Message.Tag))
                    {
                        var message = TraceMessageSerializer.Deserialize(line.TrimEnd());
                        if (message != null)
                            HandleUpdate?.Invoke(message);
                    }
                }
            }
        }
    }
}
