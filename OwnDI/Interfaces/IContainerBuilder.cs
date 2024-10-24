using OwnDI.Descriptors;

namespace OwnDI.Interfaces
{
    public interface IContainerBuilder
    {
        void Register(ServiceDescriptor descriptor);
        IContainer Build();
    }
}
