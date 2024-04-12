using BotWorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
      options.ServiceName = nameof(DirectumCareerNightBot);
    })
    .ConfigureServices(services =>
    {
      services.AddHostedService<Worker>();
    })
    .Build();

host.Run();