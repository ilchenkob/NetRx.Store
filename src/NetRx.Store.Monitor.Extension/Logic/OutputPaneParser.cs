using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Linq;

namespace NetRx.Store.Monitor.Logic
{
    public class OutputPaneParser : IOutputPaneParser
    {
        private int prevLinesCount = 0;

        public string[] GetLastLines(TextDocument textDocument)
        {
            string[] result = new string[0];

            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var selection = textDocument.Selection;

                var linesAdded = textDocument.EndPoint.Line - prevLinesCount;

                selection.EndOfDocument();
                selection.StartOfLine(vsStartOfLineOptions.vsStartOfLineOptionsFirstColumn, true);
                selection.LineUp(true, linesAdded);

                result = selection.Text?.Split('\n');

                selection.EndOfDocument(false);

                prevLinesCount = textDocument.EndPoint.Line;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }

            return result;
        }
    }
}
