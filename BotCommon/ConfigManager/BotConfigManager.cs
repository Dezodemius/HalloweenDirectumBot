using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BotCommon;

public class BotConfigManager
{
  #region Constants

  public const string BotConfigFileName = "_config.yaml";

  #endregion

  #region Fields & Props

  private readonly IDeserializer _configDeserializer = BuildDeserializer();

  public readonly BotConfig Config;

  #endregion

  #region Methods

  private static IDeserializer BuildDeserializer()
  {
    return new DeserializerBuilder()
        .WithNamingConvention(PascalCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .WithDuplicateKeyChecking()
        .Build();
  }

  #endregion

  #region Constructors

  public BotConfigManager(Stream stream)
  {
    using (stream)
    {
      using (var reader = new StreamReader(stream))
      {
        this.Config = _configDeserializer.Deserialize<BotConfig>(reader);
      }
    }
  }

  public BotConfigManager() : this(File.OpenRead(BotConfigFileName))
  {
  }

  #endregion
}