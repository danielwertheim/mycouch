using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MyCouch.EntitySchemes.Reflections
{
    public class LambdaDynamicPropertyFactory : IDynamicPropertyFactory
    {
        public virtual DynamicProperty PropertyFor(PropertyInfo p)
        {
            return new DynamicProperty(
                new DynamicStringGetter(CreateLambdaGetter<string>(p.DeclaringType, p)), 
                new DynamicStringSetter(CreateLambdaSetter<string>(p.DeclaringType, p)));
        }

        private static Func<object, TProp> CreateLambdaGetter<TProp>(Type type, PropertyInfo property)
        {
            var objExpr = Expression.Parameter(typeof(object), "theItem");
            var castedObjExpr = Expression.Convert(objExpr, type);

            var p = Expression.Property(castedObjExpr, property);

            var lambda = Expression.Lambda<Func<object, TProp>>(p, objExpr);

            return lambda.Compile();
        }

        //private static Func<T, TProp> CreateLambdaGetter<T, TProp>(PropertyInfo property)
        //{
        //    var objExpr = Expression.Parameter(typeof(T), "theItem");

        //    var p = Expression.Property(objExpr, property);

        //    var lambda = Expression.Lambda<Func<T, TProp>>(p, objExpr);

        //    return lambda.Compile();
        //}

        private static Action<object, TProp> CreateLambdaSetter<TProp>(Type type, PropertyInfo property)
        {
            var objExpr = Expression.Parameter(typeof(object), "theItem");
            var castedObjExpr = Expression.Convert(objExpr, type);
            var parameter = Expression.Parameter(typeof(TProp), "param");
#if !NETFX_CORE
            return Expression.Lambda<Action<object, TProp>>(
                Expression.Call(castedObjExpr, property.GetSetMethod(), parameter), new[] { objExpr, parameter }).Compile();
#else
            return Expression.Lambda<Action<object, TProp>>(
                Expression.Call(castedObjExpr, property.SetMethod, parameter), new[] { objExpr, parameter }).Compile();
#endif
        }

        //private static Action<T, TProp> CreateLambdaSetter<T, TProp>(PropertyInfo property)
        //{
        //    var objExpr = Expression.Parameter(typeof(T), "theItem");
        //    var parameter = Expression.Parameter(typeof(TProp), "param");

        //    return Expression.Lambda<Action<T, TProp>>(
        //        Expression.Call(objExpr, property.GetSetMethod(), parameter), new[] { objExpr, parameter }).Compile();
        //}
    }
}