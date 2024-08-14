using Microsoft.AspNetCore.Components.Authorization;
using TurneringLibrary.Data;
using TurneringLibrary.Models;

namespace TurneringUI.Helpers;

public static class AuthenticationStateProviderHelpers
{
  public static async Task<UserModel?> GetUserFromAuth(this AuthenticationStateProvider provider, IUserData userData)
  {
    var authState = await provider.GetAuthenticationStateAsync();
    string? objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("Id"))?.Value;
    string? email = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value;
    UserModel user = new();
    user.FirstName = email;
    user.ObjectIdentifier = objectId;
    user.EmailAddress = email;
    if (string.IsNullOrWhiteSpace(objectId) == false)
    {
      return await userData.GetUserFromAuthentication(user);
    }
    return null;
  }
}
