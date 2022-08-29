// ReSharper disable UnusedMember.Global

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace km.Translate.Api.Controllers;

public sealed class ReturnedVotesDto
{
  public long votes { get; set; } = 0;
  public DateTime LastVoted { get; set; } = DateTime.Now;
}
