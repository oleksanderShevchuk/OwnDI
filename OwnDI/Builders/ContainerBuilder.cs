using OwnDI.Descriptors;
using OwnDI.Interfaces;

namespace OwnDI.Builders
{
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly List<ServiceDescriptor> descriptors = new();
        public void Register(ServiceDescriptor descriptor)
        {
            descriptors.Add(descriptor);
        }
        public IContainer Build()
        {
            return new Container.Container(descriptors);
        }

    }
}
