// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace km.Library.GenericDto;

public record RequestBaseDto
{
  public int? Page { get; init; }

  public int? PageSize { get; init; }
  public bool? Shuffle { get; init; }
}

public sealed record ResponseWithPageDto<TData>
{
  public IEnumerable<TData> Data { get; init; } = null!;
  public long NextPage { get; init; }
  public long CurrentPageSize { get; init; }
  public long TotalPageCount { get; init; }
  public long CurrentPage { get; init; }

  public ResponseWithPageDto<TNewFormat> Map<TNewFormat>(Func<TData, TNewFormat> map)
  {
    return new ResponseWithPageDto<TNewFormat>
    {
      Data = Data.Select(map).ToList(),
      NextPage = NextPage,
      CurrentPageSize = CurrentPageSize,
      TotalPageCount = TotalPageCount,
      CurrentPage = CurrentPage
    };
  }
}

public sealed record ExceptionBaseDto
{
  public ExceptionBaseDto(string message, string title, string? hint = null)
  {
    Message = message;
    Title = title;
    Hint = hint;
  }
  public string Message { get; init; }
  public string Title { get; init; }
  public string? Hint { get; init; } = string.Empty;
  public override string ToString()
  {
    return $@"{{""title"": ""{Title}"", ""message"": ""{Message}"", ""hint"": ""{Hint}"", ""dateTime"": ""{DateTime.UtcNow:R}""}}";
  }
}
