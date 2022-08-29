// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace km.Library.Exceptions;

public abstract class DataException : Exception
{
  protected DataException(
    string message,
    string? title = "Data Exception",
    string? hint = "") : base(message)
  {
    Hint = hint;
    Title = title!;
  }

  public string? Hint { get; set; }
  public string Title { get; set; }
}

// Not found exception
public sealed class NotFoundException : DataException
{
  public NotFoundException(
    string message,
    string? title = "Not Found Exception",
    string? hint = "") : base(message, title, hint)
  {
  }
}

// Already exists exception
public sealed class AlreadyExistsException : DataException
{
  public AlreadyExistsException(
    string message,
    string? title = "Already Exists Exception",
    string? hint = "") : base(message, title, hint)
  {
  }
}

public sealed class AlreadyCreatedException : DataException
{
  public AlreadyCreatedException(
    string message,
    string? title = "Content Already Created Exception",
    string? hint = "") : base(message, title, hint)
  {
  }
}

// Required exception
public class RequiredException : DataException
{
  public RequiredException(
    string message,
    string? title = "Required Exception",
    string? hint = "") : base(message, title, hint)
  {
  }
}

// Invalid exception
public class InvalidOperationException : DataException
{
  public InvalidOperationException(string message, string? title = "Invalid Operation Exception", string? hint = "") : base(message, title, hint)
  {
  }
}

public sealed class InvalidTargetException : DataException
{
  public InvalidTargetException(string message, string? title = "Invalid Target Exception", string? hint = "") : base(message, title, hint)
  {
  }
}
