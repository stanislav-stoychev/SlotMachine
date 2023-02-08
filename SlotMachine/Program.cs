using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlotMachine.Registrations;
using SlotMachine.Services;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(RegistrationUtils.RegisterDependencies)
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
IServiceProvider provider = serviceScope.ServiceProvider;

provider.GetService<IGame>()
    .Play();

await host.RunAsync();