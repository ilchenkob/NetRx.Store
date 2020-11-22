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
        internal const string ReferenceFieldMarker = "*";

        private static readonly Dictionary<string, Dictionary<string, Func<object, object>>> _gettersCache
            = new Dictionary<string, Dictionary<string, Func<object, object>>>();

        private StateWrapper()
        {
        }

        internal StateWrapper(object target, string targetTypeName)
        {
            Original = target;
            OriginalTypeName = targetTypeName;
        }

        internal static StateWrapper ForObject<T>(Func<T> func)
        {
            var result = new StateWrapper();

            Type type = typeof(T);
            if (!type.IsValueType)
                throw new InvalidStateTypeException($"'{type.FullName}' cannot have reference type. Should have 'struct' type");

            result.Original = func();
            result.OriginalTypeName = type.FullName;

            if (!_gettersCache.ContainsKey(result.OriginalTypeName))
            {
                _gettersCache.Add(result.OriginalTypeName, new Dictionary<string, Func<object, object>>());
                result.BuildGetters(type, result.OriginalTypeName);
            }

            return result;
        }

        private void BuildGetters(Type type, string prefix)
        {
            var stringType = typeof(string);
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var isString = p.PropertyType == stringType;
                var isEnumerable = p.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null && !isString;
                var isReferenceType = p.PropertyType.IsClass && !isString || (p.PropertyType.IsInterface && p.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) == null);
                
                if (isEnumerable)
                {
                    var hasImmutableInterface = p.PropertyType
                                                  .GetInterfaces()
                                                  .Any(t => t.FullName.StartsWith("System.Collections.Immutable", StringComparison.InvariantCulture));
                    if (!hasImmutableInterface)
                        throw new InvalidStatePropertyTypeException(
                            $"'{p.Name}': only types from 'System.Collections.Immutable' namespace are allowed for collections");

                    var nonValueType = p.PropertyType.GenericTypeArguments.FirstOrDefault(a => !a.IsValueType && a != stringType);
                    if (nonValueType != null)
                        throw new InvalidStatePropertyTypeException(
                            $"'{p.Name}' cannot have reference type '{nonValueType.FullName}'. Should have 'struct' type");
                }
                else if (!isReferenceType &&
                        !p.PropertyType.IsValueType &&
                          !isString &&
                          !isEnumerable)
                {

                    throw new InvalidStatePropertyTypeException($"'{p.Name}' property of {prefix} cannot have reference type. Should have 'struct' type");
                }

                var wrappedObjectParameter = Expression.Parameter(typeof(object));
                var getExpression = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(Expression.Convert(wrappedObjectParameter, type), p),
                        typeof(object)),
                    wrappedObjectParameter
                );

                string name = isEnumerable 
                    ? $"{prefix}.{p.Name}{EnumerableFieldMarker}"                     
                    : isReferenceType 
                        ? $"{prefix}.{p.Name}{ReferenceFieldMarker}"
                        : $"{prefix}.{p.Name}";

                _gettersCache[OriginalTypeName].Add(name, getExpression.Compile());

            }
        }

        public object Get(string name)
        {
            var key = _gettersCache[OriginalTypeName].ContainsKey(name) 
                ? name 
                :  _gettersCache[OriginalTypeName].ContainsKey($"{name}{EnumerableFieldMarker}") 
                    ? $"{name}{EnumerableFieldMarker}"
                    : $"{name}{ReferenceFieldMarker}";

            var propNamePart = key.Substring(OriginalTypeName.Length);
            var pointIndex = propNamePart.LastIndexOf('.');
            if (pointIndex > 0) // want to get sub-property
            {
                var currentPropertyName = OriginalTypeName;
                var result = Original;
                foreach (var field in propNamePart.Split('.').Skip(1))
                {
                    currentPropertyName = $"{currentPropertyName}.{field}";
                    result = _gettersCache[OriginalTypeName][currentPropertyName](result);
                }
                return result;
            }
            else if (pointIndex < 0) // want to get original object as it is
            {
                return Original;
            }
            else
            {
                return _gettersCache[OriginalTypeName][key](Original);
            }
        }

        public IList<string> FieldNames => _gettersCache[OriginalTypeName].Keys.ToList();

        public bool HasGeter(string name) =>
            _gettersCache[OriginalTypeName].ContainsKey(name) ||
                _gettersCache[OriginalTypeName].ContainsKey($"{name}{EnumerableFieldMarker}") ||
                _gettersCache[OriginalTypeName].ContainsKey($"{name}{ReferenceFieldMarker}") ||
                    string.Equals(name, OriginalTypeName);

        public object Original { get; private set; }

        public string OriginalTypeName { get; private set; }
    }
}
