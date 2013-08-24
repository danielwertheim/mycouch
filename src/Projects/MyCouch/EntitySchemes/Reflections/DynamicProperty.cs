namespace MyCouch.EntitySchemes.Reflections
{
    public class DynamicProperty
    {
        public DynamicStringGetter Getter { get; private set; }
        public DynamicStringSetter Setter { get; private set; }

        public DynamicProperty(DynamicStringGetter getter, DynamicStringSetter setter)
        {
            Getter = getter;
            Setter = setter;
        }
    }
}