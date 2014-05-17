using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MyCouch.EntitySchemes.Reflections
{
    public class LambdaDynamicPropertyFactory : IDynamicPropertyFactory
    {
        public virtual DynamicProperty PropertyFor(PropertyInfo property)
        {
            return new DynamicProperty(
                CreateStringGetter(property),
                CreateStringSetter(property));
        }

        protected virtual IStringGetter CreateStringGetter(PropertyInfo property)
        {
            return property != null && property.CanRead
                ? new DynamicStringGetter(CreateLambdaGetter<string>(property.DeclaringType, property))
                : new FakeStringGetter() as IStringGetter;
        }

        protected virtual IStringSetter CreateStringSetter(PropertyInfo property)
        {
            return property != null && property.CanWrite
                ? new DynamicStringSetter(CreateLambdaSetter<string>(property.DeclaringType, property))
                : new FakeStringSetter() as IStringSetter;
        }

        protected virtual Func<object, TProp> CreateLambdaGetter<TProp>(Type type, PropertyInfo property)
        {
            var objExpr = Expression.Parameter(typeof(object), "theItem");
            var castedObjExpr = Expression.Convert(objExpr, type);

            var p = Expression.Property(castedObjExpr, property);

            var lambda = Expression.Lambda<Func<object, TProp>>(p, objExpr);

            return lambda.Compile();
        }

        protected virtual Action<object, TProp> CreateLambdaSetter<TProp>(Type type, PropertyInfo property)
        {
            var objExpr = Expression.Parameter(typeof(object), "theItem");
            var castedObjExpr = Expression.Convert(objExpr, type);
            var parameter = Expression.Parameter(typeof(TProp), "param");
#if !PCL
            return Expression.Lambda<Action<object, TProp>>(
                Expression.Call(castedObjExpr, property.GetSetMethod(true), parameter), new[] { objExpr, parameter }).Compile();
#else
            return Expression.Lambda<Action<object, TProp>>(
                Expression.Call(castedObjExpr, property.SetMethod, parameter), new[] { objExpr, parameter }).Compile();
#endif
        }
    }
}