using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NetRx.Store.Tests
{
    public static class Extensions
    {
        public static T GetInstanceField<T>(this Type type, object instance, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags);
            return (T)field.GetValue(instance);
        }

        public static IEnumerable<object> GetStoreItems(this Store store)
        {
            return store.GetType().GetInstanceField<IEnumerable<object>>(store, "_items");
        }

        public static dynamic GetStoreEffects(this Store store)
        {
            return store.GetType().GetInstanceField<dynamic>(store, "_effects");
        }

        private static BindingFlags BindingFlags
            => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
    }
}
