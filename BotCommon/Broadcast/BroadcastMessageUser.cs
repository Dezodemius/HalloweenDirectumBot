using System;
using Microsoft.EntityFrameworkCore;

namespace BotCommon.Broadcast;

[PrimaryKey(nameof(Id))]
public record BroadcastMessageUser(long UserId, DateTime BroadcastTime, bool IsMessageSent, bool IsBotBlocked)
{
  public int Id { get; set; }
  public bool IsBotBlocked { get; set; } = IsBotBlocked;
  public bool IsMessageSent { get; set; } = IsMessageSent;
  public DateTime BroadcastTime { get; set; } = BroadcastTime;
  public long UserId { get; set; } = UserId;
}