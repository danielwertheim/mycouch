namespace MyCouch.Reflections
{
    public class DynamicProperty
    {
        public DynamicGetter Getter { get; private set; }
        public DynamicSetter Setter { get; private set; }

        public DynamicProperty(DynamicGetter getter, DynamicSetter setter)
        {
            Getter = getter;
            Setter = setter;
        }
    }
}