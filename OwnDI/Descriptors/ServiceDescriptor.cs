using OwnDI.Models;

namespace OwnDI.Descriptors
{
    public class ServiceDescriptor
    {
        public Type ServiceType { get; init; }
        public LifeTime LifeTime { get; init; }
    }
}
