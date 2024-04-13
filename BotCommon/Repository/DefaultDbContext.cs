using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BotCommon.Repository;

/// <summary>
/// Default DB context for EF.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public abstract class DefaultDbContext<T> 
  : DbContext
{
  #region Fields and Properties

  /// <summary>
  /// Database connection string.
  /// </summary>
  protected readonly string _connectionString;

  #endregion

  #region Methods
  
  /// <summary>
  /// Get entity.
  /// </summary>
  /// <param name="entity">Entity.</param>
  /// <returns>Found entity.</returns>
  public abstract T Get(T entity);
  
  /// <summary>
  /// Get all entities.
  /// </summary>
  /// <returns>List of all entities.</returns>
  public abstract IEnumerable<T> GetAll();

  /// <summary>
  /// Add entity.
  /// </summary>
  /// <param name="entity">Adding entity.</param>
  public virtual void Add(T entity)
  {
    SaveChangesAsync();
  }

  /// <summary>
  /// Delete entity.
  /// </summary>
  /// <param name="entity">Entity to delete.</param>
  public virtual void Delete(T entity)
  {
    SaveChangesAsync();
  }

  #endregion

  #region Base class

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(_connectionString);
  }

  #endregion

  #region Constructor

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="connectionString">Database connection string.</param>
  public DefaultDbContext(string connectionString)
  {
    _connectionString = connectionString;
    Database.EnsureCreated();

    var creator = this.GetService<IRelationalDatabaseCreator>();
    if (!creator.Exists())
      creator.CreateTables();
  }

  #endregion
}