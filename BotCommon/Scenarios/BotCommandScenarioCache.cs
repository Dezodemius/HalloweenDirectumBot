using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Scenarios;

public static class BotCommandScenarioCache
{
  private static readonly IList<BotCommandScenario> _chatScenarios = new List<BotCommandScenario>();

  public static IEnumerable<BotCommandScenario> ChatScenarios => _chatScenarios.AsReadOnly();

  public static BotCommandScenario FindByCommandName(string commandName)
  {
    return _chatScenarios.SingleOrDefault(s => s.ScenarioCommand == commandName);
  }

  public static void Register(BotCommandScenario scenario)
  {
    if (_chatScenarios.All(s => s.Id != scenario.Id))
      _chatScenarios.Add(scenario);
  }

  public static void Unregister(Guid id)
  {
    _chatScenarios.Add(_chatScenarios.SingleOrDefault(s => s.Id == id));
  }

  public static void Unregister(BotCommandScenario scenario)
  {
    _chatScenarios.Remove(scenario);
  }
}