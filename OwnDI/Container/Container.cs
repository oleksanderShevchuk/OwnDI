using OwnDI.Descriptors;
using OwnDI.Interfaces;
using OwnDI.Placeholders.Interfaces;
using System.Reflection;

namespace OwnDI.Container
{
    public class Container : IContainer
    {
        private class Scope : IScope
        {
            private readonly Container _container;
            public Scope(Container container)
            {
                _container = container;
            }
            public object Resolve(Type service)
                => _container.CreateInstance(service, this);
        }
        private Dictionary<Type, ServiceDescriptor> _descriptors;
        public Container(IEnumerable<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors.ToDictionary(x => x.ServiceType);
        }
        public IScope CreateScope()
        {
            return new Scope(this);
        }
        private object CreateInstance(Type service, IScope scope)
        {
            if (!_descriptors.TryGetValue(service, out var descriptor))
                throw new InvalidOperationException($"Service {service} is not registered.");

            if (descriptor is InstanceBasedServiceDescriptor ib)
                return ib.Instance;
            if(descriptor is FactoryBasedServiceDescriptor fb)
                return fb.Factory(scope);

            var tb = (TypeBasedServiceDescriptor)descriptor;

            var ctor = tb.ImplamentationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
            var args = ctor.GetParameters();
            var argsForCtor = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                argsForCtor[i] = CreateInstance(args[i].ParameterType, scope);
            }
            return ctor.Invoke(argsForCtor);
        }
    }
}
