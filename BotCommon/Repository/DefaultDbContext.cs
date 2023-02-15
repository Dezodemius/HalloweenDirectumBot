using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BotCommon.Repository;

public abstract class DefaultDbContext<T> : DbContext
{
  protected readonly string _connectionString;

  public abstract T Get(T entity);
  public abstract IEnumerable<T> GetAll();

  public virtual void Add(T entity)
  {
    this.SaveChangesAsync();
  }

  public virtual void Delete(T entity)
  {
    this.SaveChangesAsync();
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this._connectionString);
  }

  public DefaultDbContext(string connectionString)
  {
    this._connectionString = connectionString;
    Database.EnsureCreated();

    var creator = this.GetService<IRelationalDatabaseCreator>();
    if (!creator.Exists())
      creator.CreateTables();
  }
}