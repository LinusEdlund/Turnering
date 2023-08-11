

using Dapper;
using System.Data;
using System.Xml.Linq;
using TurneringLibrary.DbAccess;
using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public class TournamentData : ITournamentData
{
  private readonly IDataAccess _data;

  public TournamentData(IDataAccess data)
  {
    _data = data;
  }

  public async Task CreateTournament(TournamentModel t, List<MatchUpModel> matchUps)
  {
    var parameters = new DynamicParameters();
    parameters.Add("Name", t.TournamentName);
    parameters.Add("HowManyPositions", t.HowManyPositions);
    parameters.Add("HowManyRounds", t.HowManyRounds);
    parameters.Add("Users_Id", t.User.Id);
    parameters.Add("InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

    await _data.SaveData<DynamicParameters>("create_tournament", parameters);

    int tournamentId = parameters.Get<int>("InsertedId");
    t.Id = tournamentId;

    foreach (var team in t.Teams)
    {
      DynamicParameters teamParameters = new();
      teamParameters.Add("TeamName", team.TeamName);
      teamParameters.Add("InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

      await _data.SaveData<DynamicParameters>("create_team", teamParameters);
      int teamId = teamParameters.Get<int>("InsertedId");
      team.Id = teamId;


      await _data.SaveData<dynamic>("create_fk_team_tour", new { tour_id = tournamentId, team_id = team.Id });

    }

    foreach (var match in matchUps)
    {
      dynamic matchParameters = new
      {
        TeamOneId = match.TeamOne.Id,
        TeamTwoId = match.TeamTwo.Id,
        TeamOneScore = match.TeamOneScore,
        TeamTwoScore = match.TeamTwoScore,
        MatchPosition = match.MatchPosition,
        tour_id = tournamentId,
        WinnerId = match.Winner.Id,
        FreeWin = match.FreeWin
      };

      await _data.SaveData<dynamic>("create_match", matchParameters);
    }

  }

  public async Task<List<TournamentModel>> GetUsersTournaments(int userId)
  {
    var output = await _data.LoadData<TournamentModel, dynamic>("get_users_tour", new { p_id = userId });
    return output.ToList();
  }

}
