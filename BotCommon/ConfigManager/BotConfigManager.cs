using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BotCommon;

/// <summary>
/// Bot configuration manager.
/// </summary>
public class BotConfigManager
{
  #region Constants

  /// <summary>
  /// Bot configuration filename.
  /// </summary>
  public const string BotConfigFileName = "_config.yaml";

  #endregion

  #region Fields & Props

  /// <summary>
  /// YAML deserializer.
  /// </summary>
  private readonly IDeserializer _configDeserializer = BuildDeserializer();

  /// <summary>
  /// Bot configuration.
  /// </summary>
  public readonly BotConfig Config;

  #endregion

  #region Methods

  /// <summary>
  /// Builds YAML deserializer.
  /// </summary>
  /// <returns>YAML deserializer</returns>
  private static IDeserializer BuildDeserializer()
  {
    return new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .WithDuplicateKeyChecking()
        .Build();
  }

  #endregion

  #region Constructors

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="stream">Configuration source stream.</param>
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

  /// <summary>
  /// Constructor.
  /// </summary>
  public BotConfigManager()
      : this(File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BotConfigFileName)))
  {
  }

  #endregion
}