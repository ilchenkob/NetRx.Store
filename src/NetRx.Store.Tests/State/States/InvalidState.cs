using System.Collections.Generic;
using System.Collections.Immutable;

namespace NetRx.Store.Tests.State
{
    public class InvalidTypeState
    {
        public int Count { get; set; }

        public string Name { get; set; }
    }

    public struct InvalidPropertyTypeState
    {
        public string Name { get; set; }

        public InvalidTypeState Model { get; set; }
    }

    public struct InvalidListPropertyTypeState
    {
        public string Name { get; set; }

        public List<string> Items { get; set; }
    }

    public struct InvalidCollectionItemTypeState
    {
        public string Name { get; set; }

        public ImmutableArray<InvalidTypeState> Data { get; set; }
    }
}
