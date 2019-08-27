using System;
using System.Collections.Generic;
using System.Reflection;
using NetRx.Store.Effects;

namespace NetRx.Store.Tests
{
    public static class Extensions
    {
        public static T GetInstanceField<T>(this Type type, object instance, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags);
            return (T)field.GetValue(instance);
        }

        internal static IEnumerable<StoreItem> GetStoreItems(this Store store)
        {
            return store.GetType().GetInstanceField<IEnumerable<StoreItem>>(store, "_items");
        }

        internal static Dictionary<string, IList<IEffectMethodWrapper>> GetStoreEffects(this Store store)
        {
            return store.GetType().GetInstanceField<Dictionary<string, IList<IEffectMethodWrapper>>>(store, "_effects");
        }

        private static BindingFlags BindingFlags
            => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
    }
}
