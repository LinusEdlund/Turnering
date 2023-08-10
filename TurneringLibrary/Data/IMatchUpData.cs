using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public interface IMatchUpData
{
  Task<List<MatchUpModel>> GetMatchUpByTourId(int tourId);
  Task UpdateMatch(MatchUpModel match);
}