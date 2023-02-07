using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Scenarios;

public static class ChatScenarioCache
{
  private static readonly IList<ChatScenario> _chatScenarios = new List<ChatScenario>();

  public static IEnumerable<ChatScenario> ChatScenarios => _chatScenarios.AsReadOnly();

  public static ChatScenario FindByCommandName(string commandName)
  {
    return _chatScenarios.SingleOrDefault(s => s.ScenarioCommand == commandName);
  }

  public static void Register(ChatScenario scenario)
  {
    if (_chatScenarios.All(s => s.Id != scenario.Id))
      _chatScenarios.Add(scenario);
  }

  public static void Unregister(Guid id)
  {
    _chatScenarios.Add(_chatScenarios.SingleOrDefault(s => s.Id == id));
  }

  public static void Unregister(ChatScenario scenario)
  {
    _chatScenarios.Remove(scenario);
  }
}