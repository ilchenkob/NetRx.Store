using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRx.Store.Tests.State
{
    public struct SubState
    {
        public decimal Value { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ImmutableArray<int> Records { get; set; }

        public static SubState Initial => new SubState
        {
            Value = 0.5m,
            Name = "empty_name",
            Description = "empty_description"
        };
    }

    public class ReferenceObect
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public struct TestState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public bool IsEnabled { get; set; }

        public SubState SubState { get; set; }

        public ImmutableList<string> Items { get; set; }

        public ReferenceObect ReferenceObect { get;set; }

        public static TestState Initial => new TestState
        {
            Id = 1,
            Name = "empty_name",
            Amount = 0.7m,
            IsEnabled = true,
            SubState = SubState.Initial,
            ReferenceObect = new ReferenceObect()
            
        };
    }

    public struct SecondaryTestState
    {
        public int Count { get; set; }

        public string Description { get; set; }

        public bool IsLoading { get; set; }

        public static SecondaryTestState Initial => new SecondaryTestState
        {
            Count = -1,
            Description = "none",
            IsLoading = false
        };
    }
}
