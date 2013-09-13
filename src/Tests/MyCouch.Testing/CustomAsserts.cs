using System;
using System.Collections;
using System.Reflection;
using FluentAssertions;
using MyCouch.Extensions;
using Xunit;

namespace MyCouch.Testing
{
    public static class CustomAsserts
    {
        public static void AreValueEqual<T>(T expected, T actual)
        {
            AreValueEqual(typeof(T), expected, actual);
        }

        private static void AreValueEqual(Type type, object a, object b)
        {
            if (type == typeof(object))
                throw new Exception("You need to specify type to do the value equality comparision.");

            if (ReferenceEquals(a, b))
                return;

            if (a == null && b == null)
                return;

            if (a == null || b == null)
                a.Should().Be(b);

            if (type.IsEnumerableType())
            {
                var array1 = a as Array;
                array1.Should().NotBeNull();

                var array2 = b as Array;
                array2.Should().NotBeNull();

                for (var i = 0; i < array1.Length; i++)
                {
                    var v1 = array1.GetValue(i);
                    var v2 = array2.GetValue(i);
                    AreValueEqual(v1.GetType(), v1, v2);
                }
                return;
            }

            if (type.IsSimpleType())
            {
                Assert.Equal(a, b);
                return;
            }
#if !NETFX_CORE
            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyType = propertyInfo.PropertyType;
                var valueForA = propertyInfo.GetValue(a, null);
                var valueForB = propertyInfo.GetValue(b, null);

                var isSimpleType = propertyType.IsSimpleType();
                if (isSimpleType)
                    valueForA.Should().Be(valueForB, "Values in property '{0}' doesn't match.", propertyInfo.Name);
                else
                    AreValueEqual(propertyType, valueForA, valueForB);
            }
#else
            foreach (var propertyInfo in type.GetRuntimeProperties())
            {
                var propertyType = propertyInfo.PropertyType;
                var valueForA = propertyInfo.GetValue(a, null);
                var valueForB = propertyInfo.GetValue(b, null);

                var isSimpleType = propertyType.IsSimpleType();
                if (isSimpleType)
                    valueForA.Should().Be(valueForB, "Values in property '{0}' doesn't match.", propertyInfo.Name);
                else
                    AreValueEqual(propertyType, valueForA, valueForB);
            }
#endif
        }
    }
}