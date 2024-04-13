using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Scenarios;

public sealed class ActionSequenceRepository
{
  private List<SequenceActionExecutor> _userScenarios = new();

  public bool TryGet(long userId, out SequenceActionExecutor sequenceActionExecutor)
  {
    sequenceActionExecutor = Get(userId);
    return sequenceActionExecutor != null;
  }

  public SequenceActionExecutor Get(long userId)
  {
    var foundedScenario = _userScenarios.SingleOrDefault(s => s.UserId == userId);
    return foundedScenario;
  }

  public void Remove(long userId)
  {
    var foundedScenario = Get(userId);
    if (foundedScenario == null)
      return;
    foundedScenario.ActionSequence.Reset();
    _userScenarios.Remove(foundedScenario);
  }

  public void Remove(SequenceActionExecutor commandScenario)
  {
    if (commandScenario == null)
      return;
    Remove(commandScenario.UserId);
  }

  public void AddOrReplace(SequenceActionExecutor sequenceActionExecutor)
  {
    if (sequenceActionExecutor == null)
      return;
    if (TryGet(sequenceActionExecutor.UserId, out var _userCommandScenario))
      Remove(_userCommandScenario);
    _userScenarios.Add(sequenceActionExecutor);
  }
}