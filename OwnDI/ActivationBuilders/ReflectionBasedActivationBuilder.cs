using OwnDI.Descriptors;
using OwnDI.Interfaces;
using OwnDI.Placeholders.Interfaces;
using System.Reflection;

namespace OwnDI.ActivationBuilders
{
    public class ReflectionBasedActivationBuilder : IActivationBuilder
    {
        public Func<IScope, object> BuildActivation(ServiceDescriptor descriptor)
        {
            var tb = (TypeBasedServiceDescriptor)descriptor;

            var ctor = tb.ImplamentationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
            var args = ctor.GetParameters();

            return s =>
            {
                var argsForCtor = new object[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    argsForCtor[i] = s.Resolve(args[i].ParameterType);
                }
                return ctor.Invoke(argsForCtor);
            };
        }
    }
}
