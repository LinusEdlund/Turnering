
namespace TurneringLibrary.Models;
public class TournamentModel
{
  public int Id { get; set; }
  public string TournamentName { get; set; }
  public int HowManyPositions { get; set; }
  public int HowManyRounds { get; set; }
  public List<MatchUpModel>? MatchUps { get; set; }
}
