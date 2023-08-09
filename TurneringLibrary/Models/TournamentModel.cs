
using System.ComponentModel.DataAnnotations;

namespace TurneringLibrary.Models;
public class TournamentModel
{
  public int Id { get; set; }
  [Required]
  [MinLength(1)]
  public string TournamentName { get; set; } = string.Empty;
  public int HowManyPositions { get; set; }
  public int HowManyRounds { get; set; }
  public UserModel User { get; set; } = new();
  public List<MatchUpModel>? MatchUps { get; set; }
}
