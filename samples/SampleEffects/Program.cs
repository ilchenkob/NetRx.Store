using System;
using SampleEffects.State;
using SampleEffects.State.Selectors;
using actions = SampleEffects.State.Actions;

namespace SimpleEffects
{
    class Program
    {
        static void Main(string[] args)
        {
            AppStateSelector.IsLoading.Subscribe(value =>
            {
                Console.WriteLine($"Loading: {value}");
            });
            AppStateSelector.DataCategory.Subscribe(value =>
            {
                Console.WriteLine($"Data Category: {value}");
            });
            AppStateSelector.DataAmount.Subscribe(value =>
            {
                Console.WriteLine($"Data Amount: {value}");
            });
            AppStateSelector.Data.Subscribe(value =>
            {
                Console.WriteLine($"Data: count - {value.Count}, amount - {value.Amount}, catogory - {value.Category}");
            });

            Store.Instance.Dispatch(new actions.SetUsername("Test name"));
            Store.Instance.Dispatch(new actions.LoadData());

            Console.ReadLine();
        }
    }
}
