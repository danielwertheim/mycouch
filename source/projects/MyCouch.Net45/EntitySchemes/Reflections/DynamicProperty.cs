using EnsureThat;

namespace MyCouch.EntitySchemes.Reflections
{
    public class DynamicProperty
    {
        public string Name { get; private set; }
        public IStringGetter Getter { get; private set; }
        public IStringSetter Setter { get; private set; }

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