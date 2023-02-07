namespace Directum238Bot;

public class BotChatCommand
{
  public const string Start = "/start";
  public const string Wish23 = "/wish23";
}

public static class BotChatCommands
{
  public static readonly string[] Commands = new[]
  {
    BotChatCommand.Start
  };
}