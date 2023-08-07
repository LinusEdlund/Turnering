

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

  public async Task CreateTournament(TournamentModel t)
  {
    var parameters = new DynamicParameters();
    parameters.Add("Name", t.TournamentName);
    parameters.Add("HowManyPositions", t.HowManyPositions);
    parameters.Add("HowManyRounds", t.HowManyRounds);
    parameters.Add("Users_Id", t.User.Id);
    parameters.Add("InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

    await _data.SaveData("create_tournament", parameters);

    int insertedId = parameters.Get<int>("InsertedId");


  }
}
