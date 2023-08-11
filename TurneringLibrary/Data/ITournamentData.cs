using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public interface ITournamentData
{
  Task CreateTournament(TournamentModel t, List<MatchUpModel> matchUps);
  Task<List<TournamentModel>> GetUsersTournaments(int userId);
}