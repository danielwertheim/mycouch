using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MyCouch.Testing
{
    /// <summary>
    /// From http://github.com/danielwertheim/ncore
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Type EnumerableType = typeof(IEnumerable);
        private static readonly Type StringType = typeof(string);
        private static readonly Type NullableType = typeof(Nullable<>);
        private static readonly HashSet<Type> ExtraPrimitiveTypes = new HashSet<Type> { typeof(string), typeof(Guid), typeof(DateTime), typeof(Decimal) };
        private static readonly HashSet<Type> ExtraPrimitiveNullableTypes = new HashSet<Type> { typeof(Guid?), typeof(DateTime?), typeof(Decimal?) };

        public static bool IsSimpleType(this Type type)
        {
#if !NETFX_CORE
            return (type.IsGenericType == false && type.IsValueType) || type.IsPrimitive || type.IsEnum || ExtraPrimitiveTypes.Contains(type) || type.IsNullablePrimitiveType();
#else
            var ti = type.GetTypeInfo();

            return (ti.IsGenericType == false && ti.IsValueType) || ti.IsPrimitive || ti.IsEnum || ExtraPrimitiveTypes.Contains(type) || type.IsNullablePrimitiveType();
#endif
        }

        public static bool IsEnumerableType(this Type type)
        {
#if !NETFX_CORE
            return type != StringType
                && type.IsValueType == false
                && type.IsPrimitive == false
                && EnumerableType.IsAssignableFrom(type);
#else
            var ti = type.GetTypeInfo();

            return type != StringType
                && ti.IsValueType == false
                && ti.IsPrimitive == false
                && EnumerableType.GetTypeInfo().IsAssignableFrom(ti);
#endif
        }

        public static bool IsNullablePrimitiveType(this Type t)
        {
#if !NETFX_CORE
            return ExtraPrimitiveNullableTypes.Contains(t) || (t.IsValueType && t.IsGenericType && t.GetGenericTypeDefinition() == NullableType && t.GetGenericArguments()[0].IsPrimitive);
#else
            var ti = t.GetTypeInfo();
            return ExtraPrimitiveNullableTypes.Contains(t) || (ti.IsValueType && ti.IsGenericType && t.GetGenericTypeDefinition() == NullableType && ti.GenericTypeArguments[0].GetTypeInfo().IsPrimitive);
#endif
        }
    }
}