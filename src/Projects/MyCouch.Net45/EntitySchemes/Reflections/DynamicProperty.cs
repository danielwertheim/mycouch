using MyCouch.EnsureThat;

namespace MyCouch.EntitySchemes.Reflections
{
    public class DynamicProperty
    {
        public IStringGetter Getter { get; private set; }
        public IStringSetter Setter { get; private set; }

        public DynamicProperty(IStringGetter getter, IStringSetter setter)
        {
            Ensure.That(getter, "getter").IsNotNull();
            Ensure.That(setter, "setter").IsNotNull();

            Getter = getter;
            Setter = setter;
        }
    }
}