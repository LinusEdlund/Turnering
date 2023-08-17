
using System.ComponentModel.DataAnnotations;

namespace TurneringLibrary.Models;
public class TournamentModel
{
  public int Id { get; set; }
  public string TournamentName { get; set; } = string.Empty;
  public int HowManyPositions { get; set; }
  public int HowManyRounds { get; set; }
  public UserModel User { get; set; } = new();
  public List<TeamModel> Teams { get; set; } = new();
}
