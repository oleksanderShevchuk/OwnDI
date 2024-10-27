using Autofac;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using OwnDI.ActivationBuilders;
using OwnDI.Extensions;
using OwnDI.Placeholders;
using OwnDI.Placeholders.Interfaces;

namespace OwnDI.Benchmarks
{
    [MemoryDiagnoser]
    public class ContainerBenchmark
    {
        private readonly IScope _reflectionBased, _lambdaBased;
        private readonly ILifetimeScope _scope;
        private readonly IServiceScope _msdi;
        public ContainerBenchmark()
        {
            var lambdaBasedBuilder = new Builders.ContainerBuilder(new LambdaBasedActivationBuilder());
            var reflectionBasedBuilder = new Builders.ContainerBuilder(new ReflectionBasedActivationBuilder());
            InitContainer(lambdaBasedBuilder);
            InitContainer(reflectionBasedBuilder);
            _reflectionBased = reflectionBasedBuilder.Build().CreateScope();
            _lambdaBased = lambdaBasedBuilder.Build().CreateScope();
            _scope = InitAutofac();
            _msdi = InitMSDI();
        }

        private void InitContainer(Builders.ContainerBuilder builder)
        {
            builder.RegisterTransient<IService, Service>()
                .RegisterTransient<Controller, Controller>();
        }

        private ILifetimeScope InitAutofac()
        {
            var containerBuilder = new Autofac.ContainerBuilder();
            containerBuilder.RegisterType<Service>().As<IService>();
            containerBuilder.RegisterType<Controller>().AsSelf();
            return containerBuilder.Build().BeginLifetimeScope();
        }

        private IServiceScope InitMSDI()
        {
            var collection = new ServiceCollection();
            collection.AddTransient<IService, Service>();
            collection.AddTransient<Controller, Controller>();
            return collection.BuildServiceProvider().CreateScope();
        }

        [Benchmark(Baseline = true)]
        public Controller Create() => new Controller(new Service());
        [Benchmark]
        public Controller Reflection() => (Controller)_reflectionBased.Resolve(typeof(Controller));
        [Benchmark]
        public Controller Lambda() => (Controller)_lambdaBased.Resolve(typeof(Controller));
        [Benchmark]
        public Controller Autofac() => _scope.Resolve<Controller>();
        [Benchmark]
        public Controller MSDI() => _msdi.ServiceProvider.GetRequiredService<Controller>();
    }
}
