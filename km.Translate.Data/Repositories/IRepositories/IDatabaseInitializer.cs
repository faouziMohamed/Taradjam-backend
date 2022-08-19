using System.Globalization;
using System.Reflection;
using CsvHelper;
using km.Library.Exceptions;
using km.Translate.Data.Data;
using km.Translate.Data.Data.Models;
using km.Translate.Data.Data.Models.Settings;
using Microsoft.Extensions.Configuration;

namespace km.Translate.Data.Repositories.IRepositories;

public interface IDatabaseInitializer
{
  /// <summary>
  ///   Remove all data and reseed the identity from all tables and reinitialize the database.
  /// </summary>
  public Task InitializeDatabaseAsync();

  /// <summary>
  ///   Remove all data and reseed the identity from all tables.
  /// </summary>
  public Task EmptyDatabaseAsync();

  /// <summary>
  ///   Empty the database and reinitialize it. using <see cref="EmptyDatabaseAsync" /> and
  ///   <see cref="InitializeDatabaseAsync" />
  /// </summary>
  public Task ReinitializeDatabaseAsync();
}

internal sealed class DatabaseInitializer : IDatabaseInitializer
{
  private readonly LanguageRepository _languageRepository;
  private readonly PropositionRepository _propositionRepository;
  private readonly RoleRepository _roleRepository;
  private readonly SentenceRepository _sentenceRepository;
  private readonly DbSettings _settings;
  private readonly UserDetailsRepository _userDetailsRepository;
  private readonly UserRepository _userRepository;
  private readonly VoteRepository _voteRepository;

  public DatabaseInitializer(ApplicationDbContext context)
  {
    _settings = GetFaultSettings();
    _sentenceRepository = new SentenceRepository(context);
    _languageRepository = new LanguageRepository(context);
    _userDetailsRepository = new UserDetailsRepository(context);
    _userRepository = new UserRepository(context);
    _roleRepository = new RoleRepository(context);
    _propositionRepository = new PropositionRepository(context);
    _voteRepository = new VoteRepository(context);
  }

  private static string BinPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

  public async Task InitializeDatabaseAsync()
  {
    try
    {
      await EmptyDatabaseAsync();
      await InitializeLanguageTable();
      await InitializeRoleTableAsync();
      await InitializeUsersTablesAsync();
      await InitializeSentencesTableAsync();
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }


  public async Task EmptyDatabaseAsync()
  {
    // Delete all rows in all tables then truncate the tables
    await _languageRepository.ClearAndResetIdentity();
    await _voteRepository.ClearAndResetIdentity();
    await _roleRepository.ClearAndResetIdentity();
    await _userDetailsRepository.ClearAndResetIdentity();
    await _userRepository.ClearAndResetIdentity();
    await _sentenceRepository.ClearAndResetIdentity();
    await _propositionRepository.ClearAndResetIdentity();
  }

  public async Task ReinitializeDatabaseAsync()
  {
    await EmptyDatabaseAsync();
    await InitializeDatabaseAsync();
  }

  private async Task InitializeSentencesTableAsync()
  {
    try
    {
      string csvFilePath = Path.Combine(BinPath, _settings.DefaultSentencesFile);
      MakeSureCsvFileExistsOrThrow(csvFilePath);
      using var reader = new StreamReader(csvFilePath);
      using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
      IEnumerable<CsvDataModel> records = csv.GetRecords<CsvDataModel>().ToList();
      await CreateSentencesList(records);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
    }
  }
  private async Task InitializeUsersTablesAsync()
  {
    string username = _settings.DefaultUsername;
    string password = _settings.DefaultPassword;
    string email = _settings.DefaultEmail;
    int role = _settings.DefaultRoleId;

    await _userDetailsRepository.AddAsync(new UserDetails
      {
        Username = username,
        Email = email,
        Password = password,
        AccountVerified = true
      }
    );

    await _userRepository.AddAsync(new User { UserDetailsId = 1, RoleId = role });
  }
  private async Task InitializeRoleTableAsync()
  {
    List<Role> roles = _settings.RoleSettings.Select(static r =>
      new Role { RoleName = r.Name, RoleDescription = r.Description }
    ).ToList();

    await _roleRepository.AddRangeAsync(roles);
  }

  private static void MakeSureCsvFileExistsOrThrow(string csvFilePath)
  {
    if (!File.Exists(csvFilePath))
    {
      throw new NotFoundException(
        "Unable to find data to initialize the database",
        "Initial Data not found",
        "Make sure the file exists in the provided path"
      );
    }
  }

  // ReSharper disable once UnusedMethodReturnValue.Local
  private async Task<IEnumerable<Sentence>> CreateSentencesList(IEnumerable<CsvDataModel> csvRecords)
  {
    IEnumerable<Sentence> sentences = csvRecords.Select(static record => new Sentence { SentenceVo = record.Sentence, SrcLanguageId = 1 })
      .ToList();

    await _sentenceRepository.AddRangeAsync(sentences);
    return sentences;
  }

  private async Task InitializeLanguageTable()
  {
    await _languageRepository.AddAsync(new Language
      {
        LanguageName = _settings.LocaleLong,
        LanguageShortName = _settings.LocaleShort
      }
    );
  }

  private static DbSettings GetFaultSettings()
  {
    return new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .Build()
      .GetRequiredSection("DbSettings")
      .Get<DbSettings>();
  }
}
