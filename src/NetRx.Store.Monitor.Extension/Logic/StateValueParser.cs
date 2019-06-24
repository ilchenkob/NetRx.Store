using NetRx.Store.Monitor.Extension.Logic.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetRx.Store.Monitor.Extension.Logic
{
    public static class StateValueParser
    {
        public static ObservableCollection<StateNodeViewModel> ParseJsonStateValue(string jsonString)
        {
            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(jsonString);                
            }
            catch (JsonReaderException)
            {
            }
            return ParseJsonObject(jObject);
        }

        private static ObservableCollection<StateNodeViewModel> ParseJsonObject(JObject jObject)
        {
            if (jObject == null)
                return null;

            var result = new ObservableCollection<StateNodeViewModel>();
            foreach (var property in jObject.Properties())
            {
                result.Add(ParseJsonProperty(property));
            }
            return result;
        }

        private static StateNodeViewModel ParseJsonProperty(JProperty property)
        {
            var node = new StateNodeViewModel
            {
                Name = property.Name
            };

            if (property.Value.Type == JTokenType.Array)
            {
                var children = property.Value.Children();
                var count = children.Count();
                node.Value = $"[{count}]";
                if (count > 0)
                {
                    node.Childs = new ObservableCollection<StateNodeViewModel>();
                    for (int i = 0; i < count; i++)
                    {
                        var currentArrayItem = children.ElementAt(i);
                        var child = new StateNodeViewModel
                        {
                            Name = i.ToString(),
                            Value = currentArrayItem.ToString()
                        };
                        if (currentArrayItem is JObject jObj)
                        {
                            child.Value = FilterJsonString(child.Value);
                            child.Childs = ParseJsonObject(jObj);
                        }
                        node.Childs.Add(child);
                    }
                }
            }
            else
            {
                node.Value = property.Value.Type == JTokenType.Null ? "null" : property.Value.ToString();
                if (property.Value.Type == JTokenType.Object)
                {
                    node.Value = FilterJsonString(node.Value);
                }
                node.Childs = ParseJsonObject(property.Value as JObject);
            }
            return node;
        }

        private static string FilterJsonString(string json)
        {
            return json.Replace("\r\n", " ").Replace("\"", "").Replace("{  ", "{ ");
        }
    }
}
