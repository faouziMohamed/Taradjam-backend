using System.Globalization;
using System.Reflection;
using CsvHelper;
using km.Library.Exceptions;
using km.Translate.DataLib.Configs.Settings;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.Extensions.Configuration;

namespace km.Translate.DataLib.Repositories;

internal sealed class DatabaseInitializer : IDatabaseInitializer
{
  private readonly ApplicationDbContext _context;
  private readonly LanguageRepository _languageRepository;
  private readonly PropositionRepository _propositionRepository;
  private readonly RoleRepository _roleRepository;
  private readonly DbSeedDbSettings _seedDbSettings;
  private readonly SentenceRepository _sentenceRepository;
  private readonly UserDetailsRepository _userDetailsRepository;
  private readonly UserRepository _userRepository;

  public DatabaseInitializer(ApplicationDbContext context)
  {
    _context = context;
    _seedDbSettings = GetFaultSettings();
    _sentenceRepository = new SentenceRepository(context);
    _languageRepository = new LanguageRepository(context);
    _userDetailsRepository = new UserDetailsRepository(context);
    _userRepository = new UserRepository(context);
    _roleRepository = new RoleRepository(context);
    _propositionRepository = new PropositionRepository(context);
  }

  private static string BinPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

  public async Task InitializeDatabaseAsync()
  {
    try
    {
      await EmptyDatabaseAsync();
      await _context.SaveChangesAsync();
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
      string csvFilePath = Path.Combine(BinPath, _seedDbSettings.DefaultSentencesFile);
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
    string username = _seedDbSettings.DefaultUsername;
    string password = _seedDbSettings.DefaultPassword;
    string email = _seedDbSettings.DefaultEmail;
    int role = _seedDbSettings.DefaultRoleId;

    await _userDetailsRepository.AddAsync(new UserDetails
      {
        Username = username,
        Email = email,
        Password = password,
        AccountVerified = true
      }
    );

    await _context.SaveChangesAsync();
    await _userRepository.AddAsync(new User { UserDetailsId = 1, RoleId = role });
  }
  private async Task InitializeRoleTableAsync()
  {
    List<Role> roles = _seedDbSettings.RoleSettings.Select(static r =>
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
    IEnumerable<Sentence> sentences = csvRecords.Select(static record => new Sentence { SentenceVo = record.Sentence, LanguageVoId = 1 })
      .ToList();

    await _sentenceRepository.AddRangeAsync(sentences);
    return sentences;
  }

  private async Task InitializeLanguageTable()
  {
    IEnumerable<Language> langs = _seedDbSettings.LangSettings.Select(static l => new Language
      {
        Name = l.Name,
        LongName = l.LongName,
        ShortName = l.ShortName
      }
    );

    await _languageRepository.AddRangeAsync(langs);
  }

  private static DbSeedDbSettings GetFaultSettings()
  {
    return new ConfigurationBuilder()
      .AddJsonFile("dataSettings.json")
      .Build()
      .GetRequiredSection("DbSeedDbSettings")
      .Get<DbSeedDbSettings>();
  }
}
