using System.Collections.Generic;

namespace NetRx.Store.Diagnostic
{
    internal interface ITraceMessageWriter
    {
        void Write(string actionTypeName, IList<StoreItem> states);
    }
}