using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Scenarios;

public static class ChatScenarioCache
{
  private static readonly IList<AutoChatScenario> _chatScenarios = new List<AutoChatScenario>();

  public static IEnumerable<AutoChatScenario> ChatScenarios => _chatScenarios.AsReadOnly();

  public static AutoChatScenario FindByCommandName(string commandName)
  {
    return _chatScenarios.SingleOrDefault(s => s.ScenarioCommand == commandName);
  }

  public static void Register(AutoChatScenario scenario)
  {
    if (_chatScenarios.All(s => s.Id != scenario.Id))
      _chatScenarios.Add(scenario);
  }

  public static void Unregister(Guid id)
  {
    _chatScenarios.Add(_chatScenarios.SingleOrDefault(s => s.Id == id));
  }

  public static void Unregister(AutoChatScenario scenario)
  {
    _chatScenarios.Remove(scenario);
  }
}