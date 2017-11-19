using EnsureThat;

namespace MyCouch.EntitySchemes.Reflections
{
    public class DynamicProperty
    {
        public string Name { get; }
        public IStringGetter Getter { get; }
        public IStringSetter Setter { get; }

        public DynamicProperty()
        {
            Name = null;
            Getter = new FakeStringGetter();
            Setter = new FakeStringSetter();
        }

        public DynamicProperty(string name, IStringGetter getter, IStringSetter setter)
        {
            Ensure.String.IsNotNullOrWhiteSpace(name, nameof(name));
            Ensure.Any.IsNotNull(getter, nameof(getter));
            Ensure.Any.IsNotNull(setter, nameof(setter));

            Name = name;
            Getter = getter;
            Setter = setter;
        }
    }
}