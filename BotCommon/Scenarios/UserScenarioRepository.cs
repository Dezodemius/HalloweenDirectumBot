﻿using System.Collections.Generic;
using System.Linq;

namespace BotCommon.Scenarios;

public sealed class UserScenarioRepository
{
  private List<UserCommandScenario> _userScenarios = new List<UserCommandScenario>();

  public bool TryGet(long userId, out UserCommandScenario userCommandScenario)
  {
    userCommandScenario = this.Get(userId);
    return userCommandScenario != null;
  }

  public UserCommandScenario Get(long userId)
  {
    var foundedScenario = this._userScenarios.SingleOrDefault(s => s.UserId == userId);
    return foundedScenario;
  }

  public void Remove(long userId)
  {
    var foundedScenario = this.Get(userId);
    foundedScenario.CommandScenario.Reset();
    this._userScenarios.Remove(foundedScenario);
  }

  public void Remove(UserCommandScenario commandScenario)
  {
    this.Remove(commandScenario.UserId);
  }

  public void Add(UserCommandScenario userCommandScenario)
  {
    if (userCommandScenario == null)
      return;
    this._userScenarios.Add(userCommandScenario);
  }
}