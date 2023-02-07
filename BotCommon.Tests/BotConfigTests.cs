using System.IO;
using System.Text;
using NUnit.Framework;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BotCommon.Tests;

public class Tests
{
  [Test]
  public void Test1()
  {
    const string Config = @"bot_token: 'test1'";
    var config = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .WithDuplicateKeyChecking()
        .Build()
        .Deserialize<BotConfig>(Config);

    BotConfigManager configManager;
    using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Config)))
    {
      configManager = new BotConfigManager(memoryStream);
    }
    var config2 = configManager.Config;

    Assert.Multiple(() =>
    {
      Assert.That(string.IsNullOrEmpty(config2.BotToken), Is.False);
      Assert.That(config2.BotToken, Is.EqualTo(config.BotToken));
    });
  }
}