using OwnDI.ActivationBuilders;
using OwnDI.Descriptors;
using OwnDI.Interfaces;

namespace OwnDI.Builders
{
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly List<ServiceDescriptor> descriptors = new();
        private readonly IActivationBuilder _builder;

        public ContainerBuilder(IActivationBuilder builder) { 
            _builder = builder;
        }
        public void Register(ServiceDescriptor descriptor)
        {
            descriptors.Add(descriptor);
        }
        public IContainer Build()
        {
            return new Container.Container(descriptors, _builder);
        }

    }
}
