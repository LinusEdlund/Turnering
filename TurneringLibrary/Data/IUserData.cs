using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public interface IUserData
{
  Task CreateUser(UserModel user);
  Task<UserModel?> GetUserFromAuthentication(UserModel user);
  Task<UserModel?> GetUserFromTournament(int tourId);
  Task UpdateUser(UserModel user);
}