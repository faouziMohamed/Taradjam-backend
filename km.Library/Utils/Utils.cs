using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace km.Library.Utils;

static public class Utils
{
  static public TSettings GetConfig<TSettings>(string configFile = "appsettings.json")
  {
    return new ConfigurationBuilder()
      .AddJsonFile(configFile)
      .Build()
      .GetRequiredSection(typeof(TSettings).Name)
      .Get<TSettings>();
  }

  /**
   * <summary>
   *   Generate the same id (hash) for the same string of 64 characters
   *   Size can be positive or negative
   *   <ul>
   *     <li>
   *       positive: the id will be the firsts size characters
   *     </li>
   *     <li>negative: the id will be the last size characters</li>
   *   </ul>
   * </summary>
   * <param name="str">string to be hashed</param>
   * <param name="size">Size of the hash</param>
   * <returns>
   *   <ul>
   *     <li> ShortHash: a short hash of size 24 bytes(by default)</li>
   *     <li> LongHash: a long hash of size 64 bytes</li>
   *   </ul>
   * </returns>
   */
  static public (string ShortHash, string LongHash) GenerateUniqueId(this string str, int size = 24)
  {
    using var sha256 = SHA256.Create();
    byte[] secretBytes = Encoding.UTF8.GetBytes(str);
    byte[] secretHash = sha256.ComputeHash(secretBytes);
    string longHash = Convert.ToHexString(secretHash).ToLowerInvariant();
    string shortHash = size > 0 ? longHash[..size] : longHash[^-size..];
    return (shortHash, longHash);
  }
}
