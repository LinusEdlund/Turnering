
namespace TurneringLibrary.Models;
public class TournamentModel
{
  public int Id { get; set; }
  public string TournamentName { get; set; } = "test";
  public int HowManyPositions { get; set; }
  public int HowManyRounds { get; set; }
  public UserModel User { get; set; } = new();
  public List<MatchUpModel>? MatchUps { get; set; }
}
