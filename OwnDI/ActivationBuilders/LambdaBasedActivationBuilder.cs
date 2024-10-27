using OwnDI.Descriptors;
using OwnDI.Interfaces;
using OwnDI.Placeholders.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace OwnDI.ActivationBuilders
{
    public class LambdaBasedActivationBuilder : BaseActivationBuilder
    {
        private static readonly MethodInfo ResolveMethod = typeof(IScope).GetMethod("Resolve");
        public override Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor tb, ConstructorInfo ctor, ParameterInfo[] args)
        {
            var scopeParameter = Expression.Parameter(typeof(IScope), "scope");

            var ctorArgs = args.Select(x =>
                 Expression.Convert(Expression.Call(scopeParameter, ResolveMethod,
                   Expression.Constant(x.ParameterType)), x.ParameterType));

            var @new = Expression.New(ctor, ctorArgs);

            var lambda = Expression.Lambda<Func<IScope, object>>(@new, scopeParameter);

            return lambda.Compile();
        }
    }
}
