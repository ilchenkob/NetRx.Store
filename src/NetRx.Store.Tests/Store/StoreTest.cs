using NetRx.Store.Effects;
using NetRx.Store.Exceptions;
using NetRx.Store.Tests.State;
using NetRx.Store.Tests.State.Effects;
using NetRx.Store.Tests.State.Reducers;
using NetRx.Store.Tests.State.TestStateActions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetRx.Store.Tests
{
    public class StoreTest
    {
        [Fact]
        public void WithState_Should_Throw_InvalidStateTypeException_When_Reference_state_type_passed()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidTypeState(), new InvalidTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStateTypeException>(exception);
            Assert.Contains(nameof(InvalidTypeState), exception.Message);
        }

        [Fact]
        public void WithState_Should_Throw_InvalidStatePropertyTypeException_When_Reference_property_type_passed()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidPropertyTypeState(), new InvalidPropertyTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStatePropertyTypeException>(exception);
            Assert.Contains(nameof(InvalidPropertyTypeState.Model), exception.Message);
        }

        [Fact]
        public void WithState_Should_Throw_InvalidStatePropertyTypeException_When_not_Immutable_collection_passed()
        {
            var exception = Record.Exception(
                () => Store.Create().WithState(new InvalidListPropertyTypeState(), new InvalidListPropertyTypeStateReducer())
            );

            Assert.NotNull(exception);
            Assert.IsType<InvalidStatePropertyTypeException>(exception);
            Assert.Contains(nameof(InvalidListPropertyTypeState.Items), exception.Message);
        }

        [Fact]
        public void WithState_Should_Throw_InvalidStatePropertyTypeException_When_Reference_type_passed_as_collection_generic_type()
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

            var items = store.GetStoreItems();
            Assert.NotNull(items);
            Assert.Equal(2, items.Count());
            Assert.NotNull(items.ElementAt(0));
            Assert.NotNull(items.ElementAt(1));
        }

        [Fact]
        public void Store_Should_have_effects_When_WithEffects_called()
        {
            var store = Store.Create()
                            .WithState(TestState.Initial, new TestStateReducer())
                            .WithEffects(new Effect[] { new SetItemsEffect() });

            var effects = store.GetStoreEffects();
            Assert.NotNull(effects);
            Assert.Single(effects.Keys);
            Assert.Equal(typeof(SetItemsAction).FullName, effects.Keys.First());
        }

        [Fact]
        public void Store_Should_have_two_effects_When_WithEffects_called_twice()
        {
            var store = Store.Create()
                            .WithState(TestState.Initial, new TestStateReducer())
                            .WithEffects(new Effect[] { new SetItemsEffect() })
                            .WithEffects(new Effect[] { new LoadItemsEffect() });

            var effects = store.GetStoreEffects();
            Assert.NotNull(effects);
            Assert.Equal(2, effects.Keys.Count());
            Assert.Equal(typeof(SetItemsAction).FullName, effects.Keys.ElementAt(0));
            Assert.Equal(typeof(LoadItemsAction).FullName, effects.Keys.ElementAt(1));
        }

        [Fact]
        public void Store_Should_have_effects_grouped_by_input_action_When_WithEffects_called()
        {
            var store = Store.Create()
                            .WithState(TestState.Initial, new TestStateReducer())
                            .WithEffects(new Effect[]
                            {
                                new SetItemsEffect(),
                                new LoadItemsEffect(),
                                new LogItemsLoadingEffect()
                            });

            var effects = store.GetStoreEffects();
            Assert.NotNull(effects);
            Assert.Equal(2, effects.Keys.Count());
            Assert.Equal(typeof(SetItemsAction).FullName, effects.Keys.ElementAt(0));
            Assert.Equal(typeof(LoadItemsAction).FullName, effects.Keys.ElementAt(1));
            Assert.Single(effects[typeof(SetItemsAction).FullName]);
            Assert.Equal(2, effects[typeof(LoadItemsAction).FullName].Count);
        }

        [Fact]
        public void Store_Should_not_have_effects_When_WithoutEffects_called()
        {
            var store = Store.Create()
                            .WithState(TestState.Initial, new TestStateReducer())
                            .WithEffects(new Effect[] { new SetItemsEffect() });

            var effects = store.GetStoreEffects();
            Assert.NotNull(effects);
            Assert.Single(effects);

            var storeWithoutEffects = store.WithoutEffects();

            effects = storeWithoutEffects.GetStoreEffects();
            Assert.NotNull(effects);
            Assert.Empty(effects);
        }

        [Fact]
        public void Dispatch_Should_call_Reducer_Reduce_and_Effect_Invoke_method_When_called()
        {
            var reducer = new TestStateReducer();
            var effect = new SetItemsEffect();
            var store = Store.Create()
                            .WithState(TestState.Initial, reducer)
                            .WithEffects(new Effect[] { effect });

            store.Dispatch(new SetItemsAction(new List<string>()));

            Assert.Single(reducer.ReduceCalls.Where(c => c == typeof(SetItemsAction).FullName));
            Assert.Equal(1, effect.CallCount);
        }
    }
}
