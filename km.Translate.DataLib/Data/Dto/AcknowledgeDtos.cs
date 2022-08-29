namespace km.Translate.DataLib.Data.Dto;

public sealed class AcknowledgeDto
{
  public string Status { get; set; }
  public string Message { get; set; }

  public override string ToString()
  {
    return $@"{{""status"":""{Status}"",""message"":""{Message}""}}";
  }
}
