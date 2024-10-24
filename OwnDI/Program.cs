using OwnDI.Builders;
using OwnDI.Extensions;
using OwnDI.Interfaces;
using OwnDI.Placeholders;
using OwnDI.Placeholders.Interfaces;

//IContainerBuilder builder;
//builder.RegisterSingleton<IService, Service>()
//    .RegisterTransient<IHelper>(s => new Helper())
//    .RegisterSingleton<IAnotherService>(AnotherServiceInstance.Instance);

IContainerBuilder builder = new ContainerBuilder();
var container = builder.RegisterTransient<IService, Service>()
    .RegisterScoped<Controller, Controller>()
    .Build();

var scope = container.CreateScope();
var controller = scope.Resolve(typeof(Controller));
Console.ReadLine();