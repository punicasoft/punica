namespace Punica.Dynamic
{
    public class AnonymousProperty
    {
        public string Name { get; init; }
        public Type Type { get; init; }

        public AnonymousProperty(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
