using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BotCommon.Scenarios;

public static class BotCommandScenarioCache
{
  private static readonly IDictionary<long, BotCommandScenario> _chatScenarios = new Dictionary<long, BotCommandScenario>();

  public static ReadOnlyDictionary<long, BotCommandScenario> ChatScenarios => _chatScenarios.AsReadOnly();

  public static KeyValuePair<long, BotCommandScenario> FindByCommandName(long userid, string commandName)
  {
    return _chatScenarios.SingleOrDefault(s => s.Key == userid && s.Value.ScenarioCommand == commandName);
  }

  public static void Register(long userid, BotCommandScenario scenario)
  {
    if (_chatScenarios.All(s => s.Key == userid && s.Value.Id != scenario.Id))
      _chatScenarios.Add(new KeyValuePair<long, BotCommandScenario>(userid, scenario) );
  }

  public static void Unregister(long userId, Guid id)
  {
    _chatScenarios.Add(_chatScenarios.SingleOrDefault(s => s.Key == userId && s.Value.Id == id));
  }

  public static void Unregister(long userId, BotCommandScenario scenario)
  {
    var scenarioToRemove = _chatScenarios.SingleOrDefault(s => s.Key == userId);
    _chatScenarios.Remove(scenarioToRemove);
  }
}