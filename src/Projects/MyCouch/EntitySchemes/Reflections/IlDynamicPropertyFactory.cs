#if !NETFX_CORE
using System;
using System.Reflection;
using System.Reflection.Emit;
using EnsureThat;

namespace MyCouch.EntitySchemes.Reflections
{
    public class IlDynamicPropertyFactory : IDynamicPropertyFactory
    {
        protected static readonly Type VoidType = typeof(void);
        protected static readonly Type IlGetterType = typeof(Func<object, string>);
        protected static readonly Type IlSetterType = typeof(Action<object, string>);

        public virtual DynamicProperty PropertyFor(PropertyInfo property)
        {
            Ensure.That(property, "property").IsNotNull();

            return new DynamicProperty(GetterFor(property), SetterFor(property));
        }

        protected virtual DynamicStringGetter GetterFor(PropertyInfo property)
        {
            return new DynamicStringGetter(CreateIlGetter(property));
        }

        protected virtual DynamicStringSetter SetterFor(PropertyInfo property)
        {
            var ilSetter = CreateIlSetter(property);
            if (ilSetter == null)
                return null;

            return new DynamicStringSetter(ilSetter);
        }

        protected virtual Func<object, string> CreateIlGetter(PropertyInfo property)
        {
            var propGetMethod = property.GetGetMethod(true);
            if (propGetMethod == null)
                return null;

            var getter = CreateDynamicGetMethod(property);

            var generator = getter.GetILGenerator();
            generator.DeclareLocal(typeof(object));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, property.DeclaringType);
            generator.EmitCall(OpCodes.Callvirt, propGetMethod, null);

            if (!property.PropertyType.IsClass)
                generator.Emit(OpCodes.Box, property.PropertyType);

            generator.Emit(OpCodes.Ret);

            return (Func<object, string>)getter.CreateDelegate(IlGetterType);
        }

        protected virtual DynamicMethod CreateDynamicGetMethod(PropertyInfo property)
        {
            var args = new[] { typeof(object) };
            var name = string.Format("_{0}{1}_", "Get", property.Name);
            var returnType = property.PropertyType;

            return !property.DeclaringType.IsInterface
                ? new DynamicMethod(
                    name,
                    returnType,
                    args,
                    property.DeclaringType,
                    true)
                : new DynamicMethod(
                    name,
                    returnType,
                    args,
                    property.Module,
                    true);
        }

        protected virtual Action<object, string> CreateIlSetter(PropertyInfo property)
        {
            var propSetMethod = property.GetSetMethod(true);
            if (propSetMethod == null)
                return null;

            var setter = CreateDynamicSetMethod(property);

            var generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, property.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            generator.Emit(property.PropertyType.IsClass
                ? OpCodes.Castclass
                : OpCodes.Unbox_Any,
                property.PropertyType);

            generator.EmitCall(OpCodes.Callvirt, propSetMethod, (Type[])null);
            generator.Emit(OpCodes.Ret);

            return (Action<object, string>)setter.CreateDelegate(IlSetterType);
        }

        protected virtual DynamicMethod CreateDynamicSetMethod(PropertyInfo property)
        {
            var args = new[] { typeof(object), property.PropertyType };
            var name = string.Format("_{0}{1}_", "Set", property.Name);
            var returnType = VoidType;

            return !property.DeclaringType.IsInterface
                ? new DynamicMethod(
                    name,
                    returnType,
                    args,
                    property.DeclaringType,
                    true)
                : new DynamicMethod(
                    name,
                    returnType,
                    args,
                    property.Module,
                    true);
        }
    }
}
#endif