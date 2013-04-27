using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCouch.Extensions
{
    /// <summary>
    /// From http://github.com/danielwertheim/ncore
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Type EnumerableType = typeof(IEnumerable);
        private static readonly Type DictionaryType = typeof(IDictionary);
        private static readonly Type DictionaryOfTType = typeof(IDictionary<,>);
        private static readonly Type KeyValuePairType = typeof(KeyValuePair<,>);
        private static readonly Type EnumType = typeof(Enum);

        private static readonly Type StringType = typeof(string);
        private static readonly Type DateTimeType = typeof(DateTime);
        private static readonly Type BoolType = typeof(bool);
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Type CharType = typeof(char);

        private static readonly Type ByteType = typeof(byte);
        private static readonly Type ShortType = typeof(short);
        private static readonly Type IntType = typeof(int);
        private static readonly Type LongType = typeof(long);

        private static readonly Type SingleType = typeof(Single);
        private static readonly Type FloatType = typeof(float);
        private static readonly Type DecimalType = typeof(decimal);
        private static readonly Type DoubleType = typeof(double);

        private static readonly Type NullableType = typeof(Nullable<>);

        private static readonly Type NullableDateTimeType = typeof(DateTime?);
        private static readonly Type NullableGuidType = typeof(Guid?);
        private static readonly Type NullableBoolType = typeof(bool?);
        private static readonly Type NullableCharType = typeof(Char?);

        private static readonly Type NullableByteType = typeof(byte?);
        private static readonly Type NullableShortType = typeof(short?);
        private static readonly Type NullableIntType = typeof(int?);
        private static readonly Type NullableLongType = typeof(long?);

        private static readonly Type NullableSingleType = typeof(Single?);
        private static readonly Type NullableFloatType = typeof(float?);
        private static readonly Type NullableDecimalType = typeof(decimal?);
        private static readonly Type NullableDoubleType = typeof(double?);

        private static readonly HashSet<Type> ExtraPrimitiveTypes = new HashSet<Type> { typeof(string), typeof(Guid), typeof(DateTime), typeof(Decimal) };
        private static readonly HashSet<Type> ExtraPrimitiveNullableTypes = new HashSet<Type> { typeof(Guid?), typeof(DateTime?), typeof(Decimal?) };
        private static readonly HashSet<Type> UnsignedTypes = new HashSet<Type> { typeof(ushort), typeof(uint), typeof(ulong) };
        private static readonly HashSet<Type> NullableUnsignedTypes = new HashSet<Type> { typeof(ushort?), typeof(uint?), typeof(ulong?) };

        public static bool IsSimpleType(this Type type)
        {
            return (type.IsGenericType == false && type.IsValueType) || type.IsPrimitive || type.IsEnum || ExtraPrimitiveTypes.Contains(type) || type.IsNullablePrimitiveType();
        }

        public static bool IsKeyValuePairType(this Type type)
        {
            return type.IsGenericType && type.IsValueType && type.GetGenericTypeDefinition() == KeyValuePairType;
        }

        public static bool IsNumericType(this Type type)
        {
            return
                IsAnyIntegerNumberType(type) ||
                IsAnyFractalNumberType(type);
        }

        public static bool IsAnyIntegerNumberType(this Type type)
        {
            return type.IsAnySignedIntegerNumberType() || type.IsAnyUnsignedType();
        }

        public static bool IsAnySignedIntegerNumberType(this Type type)
        {
            return type.IsAnyIntType()
                || type.IsAnyLongType()
                || type.IsAnyShortType()
                || type.IsAnyByteType();
        }

        public static bool IsAnyFractalNumberType(this Type type)
        {
            return type.IsAnyDoubleType()
                || type.IsAnyDecimalType()
                || type.IsAnySingleType()
                || type.IsAnyFloatType();
        }

        public static bool IsEnumerableType(this Type type)
        {
            return type != StringType
                && type.IsValueType == false
                && type.IsPrimitive == false
                && EnumerableType.IsAssignableFrom(type);
        }

        public static bool IsEnumerableBytesType(this Type type)
        {
            if (!IsEnumerableType(type))
                return false;

            var elementType = GetEnumerableElementType(type);

            return elementType.IsByteType() || elementType.IsNullableByteType();
        }

        public static Type GetEnumerableElementType(this Type type)
        {
            var elementType = (type.IsGenericType ? ExtractEnumerableGenericType(type) : type.GetElementType());
            if (elementType != null)
                return elementType;

            if (type.BaseType.IsEnumerableType())
                elementType = type.BaseType.GetEnumerableElementType();

            return elementType;
        }

        private static Type ExtractEnumerableGenericType(Type type)
        {
            var generics = type.GetGenericArguments();

            if (generics.Length == 1)
                return generics[0];

            if (generics.Length == 2 && (DictionaryType.IsAssignableFrom(type) || type.GetGenericTypeDefinition() == DictionaryOfTType))
                return KeyValuePairType.MakeGenericType(generics[0], generics[1]);

            throw new Exception("When extracting generic element type from enumerables, a maximum of two generic arguments are supported, which then are supposed to belong to KeyValuePair<TKey, TValue>.");
        }

        public static bool IsStringType(this Type t)
        {
            return t == StringType;
        }

        public static bool IsDateTimeType(this Type t)
        {
            return t == DateTimeType;
        }

        public static bool IsAnyDateTimeType(this Type t)
        {
            return IsDateTimeType(t) || IsNullableDateTimeType(t);
        }

        public static bool IsBoolType(this Type t)
        {
            return t == BoolType;
        }

        public static bool IsAnyBoolType(this Type t)
        {
            return IsBoolType(t) || IsNullableBoolType(t);
        }

        public static bool IsDecimalType(this Type t)
        {
            return t == DecimalType;
        }

        public static bool IsAnyDecimalType(this Type t)
        {
            return IsDecimalType(t) || IsNullableDecimalType(t);
        }

        public static bool IsSingleType(this Type t)
        {
            return t == SingleType;
        }

        public static bool IsAnySingleType(this Type t)
        {
            return IsSingleType(t) || IsNullableSingleType(t);
        }

        public static bool IsFloatType(this Type t)
        {
            return t == FloatType;
        }

        public static bool IsAnyFloatType(this Type t)
        {
            return IsFloatType(t) || IsNullableFloatType(t);
        }

        public static bool IsDoubleType(this Type t)
        {
            return t == DoubleType;
        }

        public static bool IsAnyDoubleType(this Type t)
        {
            return IsDoubleType(t) || IsNullableDoubleType(t);
        }

        public static bool IsLongType(this Type t)
        {
            return t == LongType;
        }

        public static bool IsAnyLongType(this Type t)
        {
            return IsLongType(t) || IsNullableLongType(t);
        }

        public static bool IsGuidType(this Type t)
        {
            return t == GuidType;
        }

        public static bool IsAnyGuidType(this Type t)
        {
            return IsGuidType(t) || IsNullableGuidType(t);
        }

        public static bool IsIntType(this Type t)
        {
            return t == IntType;
        }

        public static bool IsAnyIntType(this Type t)
        {
            return IsIntType(t) || IsNullableIntType(t);
        }

        public static bool IsByteType(this Type t)
        {
            return t == ByteType;
        }

        public static bool IsAnyByteType(this Type t)
        {
            return IsByteType(t) || IsNullableByteType(t);
        }

        public static bool IsShortType(this Type t)
        {
            return t == ShortType;
        }

        public static bool IsAnyShortType(this Type t)
        {
            return IsShortType(t) || IsNullableShortType(t);
        }

        public static bool IsCharType(this Type t)
        {
            return t == CharType;
        }

        public static bool IsAnyCharType(this Type t)
        {
            return IsCharType(t) || IsNullableCharType(t);
        }

        public static bool IsEnumType(this Type t)
        {
            return (t.BaseType == EnumType) || t.IsEnum;
        }

        public static bool IsAnyEnumType(this Type t)
        {
            return IsEnumType(t) || IsNullableEnumType(t);
        }

        public static bool IsNullablePrimitiveType(this Type t)
        {
            return ExtraPrimitiveNullableTypes.Contains(t) || (t.IsValueType && t.IsGenericType && t.GetGenericTypeDefinition() == NullableType && t.GetGenericArguments()[0].IsPrimitive);
        }

        public static bool IsNullableDateTimeType(this Type t)
        {
            return t == NullableDateTimeType;
        }

        public static bool IsNullableDecimalType(this Type t)
        {
            return t == NullableDecimalType;
        }

        public static bool IsNullableSingleType(this Type t)
        {
            return t == NullableSingleType;
        }

        public static bool IsNullableFloatType(this Type t)
        {
            return t == NullableFloatType;
        }

        public static bool IsNullableDoubleType(this Type t)
        {
            return t == NullableDoubleType;
        }

        public static bool IsNullableBoolType(this Type t)
        {
            return t == NullableBoolType;
        }

        public static bool IsNullableGuidType(this Type t)
        {
            return t == NullableGuidType;
        }

        public static bool IsNullableShortType(this Type t)
        {
            return t == NullableShortType;
        }

        public static bool IsNullableIntType(this Type t)
        {
            return t == NullableIntType;
        }

        public static bool IsNullableByteType(this Type t)
        {
            return t == NullableByteType;
        }

        public static bool IsNullableLongType(this Type t)
        {
            return t == NullableLongType;
        }

        public static bool IsNullableCharType(this Type t)
        {
            return t == NullableCharType;
        }

        public static bool IsNullableEnumType(this Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == NullableType)
            {
                t = Nullable.GetUnderlyingType(t);
                return t.IsEnumType();
            }

            return false;
        }

        public static bool IsAnyUnsignedType(this Type t)
        {
            return t.IsUnsignedType() || t.IsNullableUnsignedType();
        }

        public static bool IsUnsignedType(this Type t)
        {
            return t.IsValueType && UnsignedTypes.Contains(t);
        }

        public static bool IsNullableUnsignedType(this Type t)
        {
            return t.IsValueType && NullableUnsignedTypes.Contains(t);
        }
    }
}