using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetRx.Store.Exceptions;

namespace NetRx.Store
{
    internal sealed class StateWrapper
    {
        internal const string EnumerableFieldMarker = "#";

        private static readonly Dictionary<string, Dictionary<string, Func<object, object>>> _gettersCache
            = new Dictionary<string, Dictionary<string, Func<object, object>>>();

        internal StateWrapper(object target)
        {
            Original = target;
            Type type = target.GetType();
            if (!type.IsValueType)
                throw new InvalidStateTypeException($"{type.FullName} cannot have reference type. Should have struct type");

            OriginalTypeName = type.FullName;

            if (!_gettersCache.ContainsKey(OriginalTypeName))
            {
                _gettersCache.Add(OriginalTypeName, new Dictionary<string, Func<object, object>>());
                BuildGetters(type, OriginalTypeName);
            }
        }

        private void BuildGetters(Type type, string prefix)
        {
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var isString = string.Equals(p.PropertyType.FullName, "System.String", StringComparison.InvariantCultureIgnoreCase);
                var isEnumerable = p.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null && !isString;
                if (!p.PropertyType.IsValueType &&
                    !isString &&
                    !isEnumerable)
                    throw new InvalidStatePropertyTypeException($"Property {p.PropertyType.FullName} cannot have reference type. Should have struct type");

                var wrappedObjectParameter = Expression.Parameter(typeof(object));
                var valueParameter = Expression.Parameter(typeof(object));

                var getExpression = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(wrappedObjectParameter, type), p),
                        typeof(object)),
                    wrappedObjectParameter);

                string name = isEnumerable ? $"{prefix}.{p.Name}{EnumerableFieldMarker}" : $"{prefix}.{p.Name}";

                _gettersCache[OriginalTypeName].Add(name, getExpression.Compile());

                if (!p.PropertyType.FullName.StartsWith("System.", StringComparison.InvariantCulture)
                    && !isEnumerable)
                {
                    BuildGetters(p.PropertyType, name);
                }
            }
        }

        public object Get(string name)
        {
            var key = _gettersCache[OriginalTypeName].ContainsKey(name) ? name : $"{name}{EnumerableFieldMarker}";
            object result;
            var propNamePart = key.Substring(OriginalTypeName.Length);
            var pointIndex = propNamePart.LastIndexOf('.');
            if (pointIndex > 0) // want to get sub-property
            {
                var currentPropertyName = OriginalTypeName;
                result = Original;
                foreach (var field in propNamePart.Split('.').Skip(1))
                {
                    currentPropertyName = $"{currentPropertyName}.{field}";
                    result = _gettersCache[OriginalTypeName][currentPropertyName](result);
                }
            }
            else if (pointIndex < 0) // want to get original object as it is
            {
                return Original;
            }
            else
            {
                result = _gettersCache[OriginalTypeName][key](Original);
            }
            return result;
        }

        public IList<string> FieldNames => _gettersCache[OriginalTypeName].Keys.ToList();

        public bool HasGeter(string name) =>
            _gettersCache[OriginalTypeName].ContainsKey(name) ||
                _gettersCache[OriginalTypeName].ContainsKey($"{name}{EnumerableFieldMarker}") ||
                    string.Equals(name, OriginalTypeName);

        public object Original { get; private set; }

        public string OriginalTypeName { get; private set; }
    }
}
