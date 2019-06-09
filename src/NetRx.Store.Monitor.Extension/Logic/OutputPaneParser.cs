using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;

namespace NetRx.Store.Monitor.Logic
{
    public class OutputPaneParser : IOutputPaneParser
    {
        public string GetLastLine(TextDocument textDocument)
        {
            string result = null;

            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var selection = textDocument.Selection;

                selection.EndOfDocument();
                selection.StartOfLine(vsStartOfLineOptions.vsStartOfLineOptionsFirstColumn, true);
                selection.LineUp(true);

                result = selection.Text;

                selection.EndOfDocument(false);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }

            return result;
        }
    }
}
