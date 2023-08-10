using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public interface ITournamentData
{
  Task CreateTournament(TournamentModel t, List<TeamModel> Teams);
  Task<List<TournamentModel>> GetUsersTournaments(int userId);
}