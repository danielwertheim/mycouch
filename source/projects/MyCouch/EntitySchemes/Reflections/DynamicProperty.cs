using MyCouch.EnsureThat;

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
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();
            Ensure.That(getter, "getter").IsNotNull();
            Ensure.That(setter, "setter").IsNotNull();

            Name = name;
            Getter = getter;
            Setter = setter;
        }
    }
}