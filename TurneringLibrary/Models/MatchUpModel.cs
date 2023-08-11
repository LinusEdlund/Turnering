

namespace TurneringLibrary.Models;
public class MatchUpModel
{
  public int Id { get; set; }
  public TeamModel TeamOne { get; set; } = new();
  public TeamModel TeamTwo { get; set; } = new();
  public int TeamOneScore { get; set; }
  public int TeamTwoScore { get; set; }
  public int MatchPosition { get; set; }
  public int Tournaments_id { get; set; }
  public TeamModel Winner { get; set; } = new();
  public bool FreeWin { get; set; } = false;
}
