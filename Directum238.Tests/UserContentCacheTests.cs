using Directum238Bot;
using Directum238Bot.Repository;
using NUnit.Framework;
using Telegram.Bot.Types.Enums;

namespace Directum238.Tests;

public class Tests
{
  private UserContentCache _contentCache;

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    _contentCache = new UserContentCache("Filename=\"Test.db\"");
  }

  [OneTimeTearDown]
  public void OneTimeTearDown()
  {
    _contentCache.Database.EnsureDeleted();
  }

  [Test]
  public void Test1()
  {
    const string expectedText = "test2";
    _contentCache.Add(new UserContent(42, "test1", MessageType.Text));
    _contentCache.Add(new UserContent(43, "test2", MessageType.Text));

    var actualText = _contentCache.GetRandomContentExceptCurrent(42, MessageType.Text).Content;

    Assert.AreEqual(expectedText, actualText);
  }

  [Test]
  public void Test2()
  {
    _contentCache.Add(new UserContent(42, "test1", MessageType.Text));
    _contentCache.Add(new UserContent(42, "test2", MessageType.Text));

    var notFoundObject = _contentCache.GetRandomContentExceptCurrent(42, MessageType.Text);

    Assert.AreEqual(null, notFoundObject);
  }
}