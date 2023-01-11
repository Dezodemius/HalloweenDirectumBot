using System.IO;
using System.Text;
using NUnit.Framework;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BotCommon.Tests;

public class Tests
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public void Test1()
  {
    const string Config = @"
bot_token: 'test1'
bot_name: 'Test bot'
bot_about: 'Some phrase in profile description'
bot_description: 'Some phrase before /start'
bot_description_picture_full_filepath: 'test21'
bot_picture_full_filepath: 'test3'
bot_commands: 'test4'";
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
        Assert.That(config2.BotCommands, Is.EqualTo(config.BotCommands));
        Assert.That(config2.BotAbout, Is.EqualTo(config.BotAbout));
        Assert.That(config2.BotName, Is.EqualTo(config.BotName));
        Assert.That(config2.BotDescription, Is.EqualTo(config.BotDescription));
        Assert.That(config2.BotToken, Is.EqualTo(config.BotToken));
        Assert.That(config2.BotPictureFullFilepath, Is.EqualTo(config.BotPictureFullFilepath));
        Assert.That(config2.BotDescriptionPictureFullFilepath, Is.EqualTo(config.BotDescriptionPictureFullFilepath));
    });

    Assert.Multiple(() =>
    {
      Assert.That(string.IsNullOrEmpty(config2.BotCommands), Is.False);
      Assert.That(string.IsNullOrEmpty(config2.BotAbout), Is.False);
      Assert.That(string.IsNullOrEmpty(config2.BotName), Is.False);
      Assert.That(string.IsNullOrEmpty(config2.BotDescription), Is.False);
      Assert.That(string.IsNullOrEmpty(config2.BotToken), Is.False);
      Assert.That(string.IsNullOrEmpty(config2.BotPictureFullFilepath), Is.False);
      Assert.That(string.IsNullOrEmpty(config2.BotDescriptionPictureFullFilepath), Is.False);
    });
  }
}