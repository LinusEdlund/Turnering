

using TurneringLibrary.DbAccess;
using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public class MatchUpData : IMatchUpData
{
  private readonly IDataAccess _data;

  public MatchUpData(IDataAccess data)
  {
    _data = data;
  }

  public async Task<List<MatchUpModel>> GetMatchUpByTourId(int tourId)
  {
    var paramValues = new { p_id = tourId };

    var output = await _data.LoadDataParam<MatchUpModel, TeamModel, TeamModel, TeamModel>("get_matchups", (match, teamOne, teamTwo, winner) => 
    {
      match.TeamOne = teamOne;
      match.TeamTwo = teamTwo;
      match.Winner = winner;
      return match;
    },
    "Id, Id, Id",
    paramValues
    );
    return output.ToList();
  }

  public Task UpdateMatch(MatchUpModel match)
  {
    dynamic param = new
    {
      match_id = match.Id,
      TeamOneId = match.TeamOne?.Id,
      TeamTwoId = match.TeamTwo?.Id,
      TeamOneScore = match.TeamOneScore,
      TeamTwoScore = match.TeamTwoScore,
      WinnerId = match.Winner?.Id
    };
    return _data.SaveData<dynamic>("update_matchup", param);
  }
}
