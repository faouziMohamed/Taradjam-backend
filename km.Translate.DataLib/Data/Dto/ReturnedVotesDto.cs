// ReSharper disable UnusedMember.Global

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace km.Translate.DataLib.Data.Dto;

public sealed class ReturnedVotesDto
{
  public long Votes { get; set; } = 0;
  public DateTime LastVoted { get; set; } = DateTime.Now;
}
