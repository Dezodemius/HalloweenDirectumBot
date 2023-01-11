using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;

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
    var config = new BotConfig
    {
        BotCommands = "/start",
        BotAbout = "about",
        BotName = "name",
        BotDescription = "description",
        BotToken = "token",
        BotPicture = "picture",
        BotDescriptionPictureFullFilepath = "descriptionPictureFullFilepath"
    };

    var jsonConfig = JsonConvert.SerializeObject(config);
    BotConfigManager configManager;
    using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonConfig)))
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
        Assert.That(config2.BotPicture, Is.EqualTo(config.BotPicture));
        Assert.That(config2.BotDescriptionPictureFullFilepath, Is.EqualTo(config.BotDescriptionPictureFullFilepath));
    });
    }
}