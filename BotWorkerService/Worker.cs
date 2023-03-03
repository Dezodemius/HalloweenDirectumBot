using System.Diagnostics;

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
      botProcess.StartInfo = new ProcessStartInfo()
      {
          FileName = @"E:\Projects\TelegramBots\Directum238Bot\bin\Debug\net7.0\Directum238Bot.exe",
          WorkingDirectory = @"E:\Projects\TelegramBots\Directum238Bot\bin\Debug\net7.0"
      };
      botProcess.Start();
      botProcess.WaitForExit();
    }
    return Task.CompletedTask;
  }
}