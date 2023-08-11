using Microsoft.AspNetCore.Components;
using TurneringLibrary.Models;
using TurneringUI.Helpers;

namespace TurneringUI.Component;

public partial class AddTournamentComponent
{
  private TournamentModel Tournament = new();
  private List<MatchUpModel> MatchUps = new();
  private UserModel? loggedInUser;

  [Parameter]
  public EventCallback<List<MatchUpModel>> TournamentMatches { get; set; }

  protected override async Task OnInitializedAsync()
  {
    loggedInUser = await authProvider.GetUserFromAuth(userData);
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      await LoadTourTeamState();
      StateHasChanged();
    }
  }

  private async Task UpdateTeamName(ChangeEventArgs e, TeamModel team)
  {
    team.TeamName = e.Value.ToString();
    await TournamentMatches.InvokeAsync(MatchUps);
    await SaveTourTeamState();
  }

  private async Task UpdateTourName(ChangeEventArgs e)
  {
    Tournament.TournamentName = e.Value.ToString();
    await SaveTourTeamState();
  }

  private async Task LoadTourTeamState()
  {
    var tournamentResults = await sessionStorage.GetAsync<TournamentModel>(nameof(Tournament));

    if (tournamentResults.Success && tournamentResults.Value is not null)
    {
      Tournament = tournamentResults.Value;

      AddingAllMatchups();
      PlacingTeamsInThereMatchUps(Tournament.Teams.Count);
      CheckingForFreeWins();

      MatchUps = MatchUps.OrderByDescending(x => x.MatchPosition).ToList();
      await TournamentMatches.InvokeAsync(MatchUps);
    }

  }

  private async Task SaveTourTeamState()
  {
    await sessionStorage.SetAsync(nameof(Tournament), Tournament);
  }

  private async void AddTeam()
  {
    TeamModel team = new();
    int teamCount = Tournament.Teams.Count + 1;
    team.TeamName = $"Team {teamCount}";
    Tournament.Teams.Add(team);
    if (teamCount < 2)
    {
      return;
    }

    GetRoundsAndPositions(teamCount);
    AddingAllMatchups();
    PlacingTeamsInThereMatchUps(teamCount);
    CheckingForFreeWins();

    MatchUps = MatchUps.OrderByDescending(x => x.MatchPosition).ToList();

    await TournamentMatches.InvokeAsync(MatchUps);

    await SaveTourTeamState();
  }

  private void GetRoundsAndPositions(int teamCount)
  {
    double rounds = Math.Ceiling(Math.Log(teamCount, 2));
    double positions = Math.Pow(2, rounds);
    Tournament.HowManyRounds = (int)rounds;
    Tournament.HowManyPositions = (int)positions - 1;
  }

  private void AddingAllMatchups()
  {
    MatchUps = new();
    for (int i = 0; i < Tournament.HowManyPositions; i++)
    {
      MatchUpModel match = new();
      match.MatchPosition = i + 1;
      MatchUps.Add(match);
    }
  }

  private void PlacingTeamsInThereMatchUps(int teamCount)
  {
    int lastFirstRoundPosition = 1;
    int firstFirstRoundPosition = Tournament.HowManyPositions;
    if (Tournament.HowManyRounds > 1)
    {
      lastFirstRoundPosition = (Tournament.HowManyPositions / 2) + 1;
    }

    for (int i = 0; i < teamCount; i++)
    {
      if (i % 2 == 0)
      {
        var updateMatch = MatchUps.First(x => x.MatchPosition == firstFirstRoundPosition);
        updateMatch.TeamOne = Tournament.Teams[i];
        firstFirstRoundPosition--;
      }
      else
      {
        var updateMatch = MatchUps.First(x => x.MatchPosition == lastFirstRoundPosition);
        updateMatch.TeamTwo = Tournament.Teams[i];
        lastFirstRoundPosition++;
      }
    }
  }

  private void CheckingForFreeWins()
  {
    for (int i = 0; i < MatchUps.Count; i++)
    {
      int match = MatchUps[i].MatchPosition;
      if (!string.IsNullOrWhiteSpace(MatchUps[i].TeamOne.TeamName) && string.IsNullOrWhiteSpace(MatchUps[i].TeamTwo.TeamName))
      {
        var matchup = MatchUps.First(x => x.MatchPosition == match);
        matchup.Winner = matchup.TeamOne;
        matchup.FreeWin = true;
        PlaceingWinner(match);
      }

      if (string.IsNullOrWhiteSpace(MatchUps[i].TeamOne.TeamName) && !string.IsNullOrWhiteSpace(MatchUps[i].TeamTwo.TeamName))
      {
        var matchup = MatchUps.First(x => x.MatchPosition == match);
        matchup.Winner = matchup.TeamTwo;
        matchup.FreeWin = true;
        PlaceingWinner(match);
      }
    }
  }

  private void PlaceingWinner(int position)
  {
    Dictionary<int, List<int>> placement = new()
          {
              {
                  1,
                  new List<int>
                  {
                      2,
                      3
                  }
              },
              {
                  2,
                  new List<int>
                  {
                      4,
                      5
                  }
              },
              {
                  3,
                  new List<int>
                  {
                      6,
                      7
                  }
              }
          };
    int winnerPosistion = placement.FirstOrDefault(x => x.Value.Contains(position)).Key;
    var currentMatchup = MatchUps.First(x => x.MatchPosition == position);
    var winnersNewMatchup = MatchUps.First(x => x.MatchPosition == winnerPosistion);
    if (position % 2 == 0)
    {
      winnersNewMatchup.TeamTwo = currentMatchup.Winner;
    }
    else
    {
      winnersNewMatchup.TeamOne = currentMatchup.Winner;
    }
  }

  private async Task RemoveTeam(TeamModel team)
  {
    Tournament.Teams.Remove(team);

    int teamCount = Tournament.Teams.Count;

    GetRoundsAndPositions(teamCount);
    AddingAllMatchups();
    PlacingTeamsInThereMatchUps(teamCount);
    CheckingForFreeWins();

    MatchUps = MatchUps.OrderByDescending(x => x.MatchPosition).ToList();

    await TournamentMatches.InvokeAsync(MatchUps);
    await SaveTourTeamState();
  }

  private async Task SaveData()
  {
    // maybe clear it here
    if (loggedInUser is not null)
    {
      Tournament.User = loggedInUser;
      await tournamentData.CreateTournament(Tournament, MatchUps);
      navigat.NavigateTo($"/View/Basic/{Tournament.Id}");
    }
    else
    {
      navigat.NavigateTo("/Identity/Account/Login", true);
    }
  }
}