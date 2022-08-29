#nullable disable
using km.Translate.DataLib.Data.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace km.Translate.DataLib.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext()
  {
  }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
  {

  }

  public DbSet<Sentence> Sentences { get; set; }
  public DbSet<Language> Languages { get; set; }

  public DbSet<Proposition> Propositions { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Role> Roles { get; set; }
  public DbSet<UserDetails> UserDetails { get; set; }
}
