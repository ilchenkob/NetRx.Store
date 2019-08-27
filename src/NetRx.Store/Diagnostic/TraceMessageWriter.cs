using NetRx.Store.Monitor.Shared;
using NetRx.Store.Monitor.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetRx.Store.Diagnostic
{
    internal class TraceMessageWriter : ITraceMessageWriter
    {
        public void Write(string actionTypeName, IList<StoreItem> states)
        {
            var stateNames = states.Select(s => s.State.OriginalTypeName.Split('.').Last()).Aggregate((a, b) => $"{a}, {b}");
            var message = TraceMessageSerializer.Serialize(new TraceMessage
            {
                StoreTypeName = stateNames,
                ActionTypeName = actionTypeName,
                StateValueJson = ConvertToStateValueJson(states)
            });
            Trace.WriteLine(message);
        }

        private string ConvertToStateValueJson(IList<StoreItem> states)
        {
            var stateProps = states
                .Select(i =>
                {
                    var stateName = i.State.OriginalTypeName.Split('.').Last();
                    var value = JsonConvert.SerializeObject(i.State.Original);
                    return $"\"{stateName}\":{value}";
                })
                .Aggregate((a, b) => $"{a},{b}");

            return $"{{{stateProps}}}";
        }
    }
}