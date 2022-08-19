using System.ComponentModel.DataAnnotations;

namespace km.Library.Repositories;

// ReSharper disable UnusedAutoPropertyAccessor.Global
public abstract class BaseEntity
{
  [Key]
  public int Id { get; set; }
}
