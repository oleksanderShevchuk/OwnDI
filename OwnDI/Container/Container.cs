using OwnDI.Descriptors;
using OwnDI.Interfaces;
using OwnDI.Placeholders.Interfaces;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;

namespace OwnDI.Container
{
    public class Container : IContainer, IDisposable, IAsyncDisposable
    {
        private class Scope : IScope
        {
            private readonly Container _container;
            private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();
            private readonly ConcurrentStack<object> _disposables = new();
            public Scope(Container container)
            {
                _container = container;
            }
            public object Resolve(Type service)
            {
                var descriptor = _container.FindDescriptor(service);
                if (descriptor.LifeTime == Models.LifeTime.Transient)
                    return CreateInstanceInternal(service);
                if (descriptor.LifeTime == Models.LifeTime.Scoped || _container._rootScope == this)
                {
                    return _scopedInstances.GetOrAdd(service, s => _container.CreateInstance(s, this));
                }
                else
                {
                    return _container._rootScope.Resolve(service);
                }
            }

            private object CreateInstanceInternal(Type service)
            {
                var result = _container.CreateInstance(service, this);
                if(result is IDisposable || result is IAsyncDisposable)
                    _disposables.Push(result);
                return result;
            }

            public void Dispose()
            {
                foreach (var desposable in _disposables)
                {
                    if (desposable is IDisposable d)
                        d.Dispose();
                    else if(desposable is IAsyncDisposable ad)
                        ad.DisposeAsync().GetAwaiter().GetResult();
                }
            }

            public async ValueTask DisposeAsync()
            {
                foreach (var desposable in _disposables)
                {
                    if (desposable is IAsyncDisposable ad)
                        await ad.DisposeAsync();
                    else if (desposable is IDisposable d)
                        d.Dispose();
                }
            }
        }

        private readonly Dictionary<Type, ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<Type, Func<IScope, object>> _buildActivators = new();
        private readonly Scope _rootScope;
        private readonly IActivationBuilder _builder;
        public Container(IEnumerable<ServiceDescriptor> descriptors, IActivationBuilder builder)
        {
            _descriptors = descriptors.ToDictionary(x => x.ServiceType);
            _rootScope = new Scope(this);
            _builder = builder;
        }
        public IScope CreateScope()
        {
            return new Scope(this);
        }

        private Func<IScope, object> BuildActivation(Type service)
        {
            if (!_descriptors.TryGetValue(service, out var descriptor))
                throw new InvalidOperationException($"Service {service} is not registered.");

            if (descriptor is InstanceBasedServiceDescriptor ib)
                return s => ib.Instance;
            if (descriptor is FactoryBasedServiceDescriptor fb)
                return fb.Factory;

            return _builder.BuildActivation(descriptor);
        }

        private object CreateInstance(Type service, IScope scope)
        {
            return _buildActivators.GetOrAdd(service, BuildActivation)(scope);
        }

        private ServiceDescriptor? FindDescriptor(Type service)
        {
            _descriptors.TryGetValue(service, out var result);
            return result;
        }

        void IDisposable.Dispose() => _rootScope.Dispose();

        public ValueTask DisposeAsync() => _rootScope.DisposeAsync();
    }
}
