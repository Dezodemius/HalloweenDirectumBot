using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BotWorkerService;

public class Worker : BackgroundService
{
  private readonly ILogger<Worker> _logger;

  public Worker(ILogger<Worker> logger)
  {
    _logger = logger;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      var botProcess = new Process();
      var botAssembly = Assembly.Load(nameof(Directum238Bot));
      var botExecutableFilepath = Path.ChangeExtension(botAssembly.Location, "exe");
      botProcess.StartInfo = new ProcessStartInfo
      {
          FileName = botExecutableFilepath,
          WorkingDirectory = Path.GetDirectoryName(botAssembly.Location)
      };
      botProcess.Start();
      botProcess.WaitForExit();
    }
    return Task.CompletedTask;
  }
}