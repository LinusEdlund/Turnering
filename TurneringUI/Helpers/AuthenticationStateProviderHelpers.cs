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
    if (string.IsNullOrWhiteSpace(objectId) == false)
    {
      return await userData.GetUserFromAuthentication(objectId);
    }
    return null;
  }
}
