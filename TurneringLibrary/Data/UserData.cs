

using TurneringLibrary.DbAccess;
using TurneringLibrary.Models;

namespace TurneringLibrary.Data;
public class UserData : IUserData
{
  private readonly IDataAccess _data;

  public UserData(IDataAccess data)
  {
    _data = data;
  }

  public async Task<UserModel?> GetUserFromAuthentication(UserModel user)
  {
    var output = await _data.LoadData<UserModel, dynamic>("get_user_auth", 
        new { auth_id = user.ObjectIdentifier });
    if (output.Count() < 1) 
    {
        await CreateUser(user);
        return user;
    }
    return output.FirstOrDefault();
  }

  public async Task CreateUser(UserModel user)
  {
    dynamic param = new
    {
      p_ObjectIdentifier = user.ObjectIdentifier,
      p_FirstName = user.FirstName,
      p_LastName = user.LastName,
      p_DisplayName = user.DisplayName,
      p_EmailAddress = user.EmailAddress
    };
    await _data.SaveData("create_user", param);
  }

  public async Task UpdateUser(UserModel user)
  {
    dynamic param = new
    {
      p_id = user.Id,
      p_ObjectIdentifier = user.ObjectIdentifier,
      p_FirstName = user.FirstName,
      p_LastName = user.LastName,
      p_DisplayName = user.DisplayName,
      p_EmailAddress = user.EmailAddress
    };


    await _data.SaveData("update_user", param);
  }

  public async Task<UserModel?> GetUserFromTournament(int tourId)
  {
    var output = await _data.LoadData<UserModel, dynamic>("get_user_from_tour_id", new { tour_id = tourId });
    return output.FirstOrDefault();
  }
}
