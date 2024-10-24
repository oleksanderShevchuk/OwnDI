using OwnDI.Placeholders.Interfaces;

namespace OwnDI.Descriptors
{
    public class FactoryBasedServiceDescriptor : ServiceDescriptor
    {
        public Func<IScope, object> Factory { get; init; }
    }
}
