
using Microsoft.AspNetCore.Components;
using TurneringLibrary.Models;
using TurneringUI.Helpers;

namespace TurneringUI.Pages;

public partial class ViewBasicTournament
{
  [Parameter]
  public int Id { get; set; }

  private List<MatchUpModel>? matchUps;
  private MatchUpModel? matchToEdit;
  private UserModel? loggedInUser;
  private UserModel? tournamentAuther;
  protected override async Task OnInitializedAsync()
  {
    matchUps = await matchUpData.GetMatchUpByTourId(Id);
    loggedInUser = await authProvider.GetUserFromAuth(userData);
    if (matchUps is not null)
    {
      int tourId = matchUps.First().Tournaments_id;
      tournamentAuther = await userData.GetUserFromTournament(tourId);
    }
  }

  private string Style(int position)
  {
    return $"position{position}-Rounds{matchUps.Count}";
  }

  private void ShowMatchUp(MatchUpModel match)
  {
    if (loggedInUser is null || loggedInUser.Id != tournamentAuther?.Id)
    {
      matchToEdit = null;
      return;
    }

    if (match.TeamOne is not null && match.TeamTwo is not null && match.Winner is null)
    {
      matchToEdit = match;
    }
    else
    {
      matchToEdit = null;
    }
  }

  private string DisplayError(MatchUpModel match)
  {
    if (loggedInUser is null || loggedInUser.Id != tournamentAuther?.Id)
    {
      matchToEdit = null;
      return "";
    }

    if (match.TeamOne is null || match.TeamTwo is null || match.Winner is not null)
    {
      return "highlight-red";
    }

    return "";
  }

  private async Task Score()
  {

    if (loggedInUser is null || loggedInUser.Id != tournamentAuther?.Id)
    {
      matchToEdit = null;
      return;
    }

    if (matchToEdit is null)
    {
      matchToEdit = null;
      return;
    }

    if (matchToEdit.TeamOneScore == matchToEdit.TeamTwoScore)
    {
      matchToEdit = null;
      return;
    }

    if (matchToEdit.TeamOneScore > matchToEdit.TeamTwoScore)
    {
      matchToEdit.Winner = matchToEdit.TeamOne;
    }
    else
    {
      matchToEdit.Winner = matchToEdit.TeamTwo;
    }

    int winnerPosistion = Rounds.placement.FirstOrDefault(x => x.Value.Contains(matchToEdit.MatchPosition)).Key;
    var winnersNewMatchup = matchUps.First(x => x.MatchPosition == winnerPosistion);
    if (matchToEdit.MatchPosition % 2 == 0)
    {
      winnersNewMatchup.TeamTwo = matchToEdit.Winner;
    }
    else
    {
      winnersNewMatchup.TeamOne = matchToEdit.Winner;
    }

    await matchUpData.UpdateMatch(matchToEdit);
    await matchUpData.UpdateMatch(winnersNewMatchup);
    matchToEdit = null;
  }
  //private bool ValidateScores()
  //{
  //  if (matchToEdit.TeamOneScore == matchToEdit.TeamTwoScore)
  //  {
  //    editContext.AddValidationMessage(() => matchToEdit.TeamTwoScore, "Scores for Team 1 and Team 2 cannot be the same.");
  //    return false;
  //  }
  //  return true;
  //}
}