#nullable disable
using km.Library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace km.Translate.DataLib.Data.Models;

[Index(nameof(RoleName), IsUnique = true)]
public sealed class Role : BaseEntity
{
  public string RoleName { get; set; } = "Basic";

  /**
   * <summary>
   *   <ul>
   *     <li>
   *       Basic: Basic Role, can Add, Edit, Approve, and Delete Translations;
   *     </li>
   *     <li>
   *       Moderator: Moderator has the same abilities as Basic, but can also add new Sentences and manage users (Add,
   *       Edit, Approve, and Delete Users);
   *     </li>
   *   </ul>
   * </summary>
   */
  public string RoleDescription { get; set; }
}
