using BenchmarkDotNet.Running;
using OwnDI.ActivationBuilders;
using OwnDI.Benchmarks;
using OwnDI.Builders;
using OwnDI.Extensions;
using OwnDI.Interfaces;
using OwnDI.Placeholders;
using OwnDI.Placeholders.Interfaces;

//IContainerBuilder builder;
//builder.RegisterSingleton<IService, Service>()
//    .RegisterTransient<IHelper>(s => new Helper())
//    .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance);

BenchmarkRunner.Run<ContainerBenchmark>();

//IContainerBuilder builder = new ContainerBuilder(new LambdaBasedActivationBuilder());
//var container = builder.RegisterTransient<IService, Service>()
//    .RegisterScoped<Controller, Controller>()
//    .Build();

//var scope = container.CreateScope();
//var controller1 = scope.Resolve(typeof(Controller));
//var controller2 = scope.Resolve(typeof(Controller));

//var scope2 = container.CreateScope();
//var controller3 = scope2.Resolve(typeof(Controller));

//if (controller1 != controller2)
//{
//    throw new InvalidOperationException();
//}

//Console.ReadLine();