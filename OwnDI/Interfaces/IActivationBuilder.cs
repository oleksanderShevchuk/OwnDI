using OwnDI.Descriptors;
using OwnDI.Placeholders.Interfaces;

namespace OwnDI.Interfaces
{
    public interface IActivationBuilder
    {
        Func<IScope, object> BuildActivation(ServiceDescriptor descriptor);
    }
}
