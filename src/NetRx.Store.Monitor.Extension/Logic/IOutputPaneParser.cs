using EnvDTE;

namespace NetRx.Store.Monitor.Logic
{
    public interface IOutputPaneParser
    {
        string[] GetLastLines(TextDocument textDocument);
    }
}
