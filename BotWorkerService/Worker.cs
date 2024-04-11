using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BotWorkerService;

public class Worker : BackgroundService
{
  private Process _botProcess;
  private readonly ILogger<Worker> _logger;

  public Worker(ILogger<Worker> logger)
  {
    _logger = logger;
    PrepareProcess();
  }

  private void PrepareProcess()
  {
    _botProcess = new Process();
    var botAssembly = Assembly.Load(nameof(DirectumCareerNightBot));
    var botExecutableFilepath = Path.ChangeExtension(botAssembly.Location, "exe");
    _botProcess.StartInfo = new ProcessStartInfo
    {
        FileName = botExecutableFilepath,
        WorkingDirectory = Path.GetDirectoryName(botAssembly.Location)
    };
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    stoppingToken.Register(() =>
    {
      if (!_botProcess.HasExited)
        _botProcess.Kill();
    });
    while (!stoppingToken.IsCancellationRequested)
    {
      _botProcess.Start();
      _botProcess.WaitForExit();
    }
    return Task.CompletedTask;
  }
}