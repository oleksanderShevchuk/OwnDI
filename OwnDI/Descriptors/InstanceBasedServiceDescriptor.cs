using OwnDI.Models;

namespace OwnDI.Descriptors
{
    public class InstanceBasedServiceDescriptor : ServiceDescriptor
    {
        public object Instance { get; init; }
        public InstanceBasedServiceDescriptor(Type serviceType, object instance)
        {
            LifeTime = LifeTime.Singleton;
            ServiceType = serviceType;
            Instance = instance;
        }
    }
}
