using System.Linq;
using Xunit;
using NetRx.Store.Exceptions;
using NetRx.Store.Tests.State;
using NetRx.Store.Tests.State.Reducers;

namespace NetRx.Store.Tests
{
    public class StoreTest
    {
        [Fact]
        public void StoreWithState_Should_Throw_InvalidStateTypeException_When_Reference_state_type_passed()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidTypeState(), new InvalidTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStateTypeException>(exception);
            Assert.Contains(nameof(InvalidTypeState), exception.Message);
        }

        [Fact]
        public void StoreWithState_Should_Throw_InvalidStatePropertyTypeException_When_Reference_property_type_passed()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidPropertyTypeState(), new InvalidPropertyTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStatePropertyTypeException>(exception);
            Assert.Contains(nameof(InvalidPropertyTypeState.Model), exception.Message);
        }

        [Fact]
        public void StoreWithState_Should_Throw_InvalidStatePropertyTypeException_When_not_Immutable_collection_passed()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidListPropertyTypeState(), new InvalidListPropertyTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStatePropertyTypeException>(exception);
            Assert.Contains(nameof(InvalidListPropertyTypeState.Items), exception.Message);
        }

        [Fact]
        public void StoreWithState_Should_Throw_InvalidStatePropertyTypeException_When_Reference_type_passed_as_collection_generic_type()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidCollectionItemTypeState(), new InvalidCollectionItemTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStatePropertyTypeException>(exception);
            Assert.Contains(nameof(InvalidCollectionItemTypeState.Data), exception.Message);
        }

        [Fact]
        public void Store_Should_have_two_states_When_WithState_called_twice()
        {
            var store = Store.Create()
                            .WithState(TestState.Initial, new TestStateReducer())
                            .WithState(SecondaryTestState.Initial, new SecondaryTestStateReducer());

            Assert.NotNull(store);

            var items = store.GetStoreItems();

            Assert.NotNull(items);
            Assert.True(items.Count() == 2);
            Assert.NotNull(items.ElementAt(0));
            Assert.NotNull(items.ElementAt(1));
        }
    }
}
