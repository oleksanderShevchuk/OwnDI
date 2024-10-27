using OwnDI.Descriptors;
using OwnDI.Interfaces;
using OwnDI.Placeholders.Interfaces;
using System.Reflection;

namespace OwnDI.ActivationBuilders
{
    public abstract class BaseActivationBuilder : IActivationBuilder
    {
        public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
        {
            var tb = (TypeBasedServiceDescriptor)descriptor;

            var ctor = tb.ImplamentationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
            var args = ctor.GetParameters();

            return BuildActivationInternal(tb, ctor, args);
        }

        public abstract Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor tb, ConstructorInfo ctor, ParameterInfo[] args);
    }
}
