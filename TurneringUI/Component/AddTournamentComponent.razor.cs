using Microsoft.AspNetCore.Components;
using TurneringLibrary.Models;

namespace TurneringUI.Component;

public partial class AddTournamentComponent
{
  private List<TeamModel> Teams = new();
  private TournamentModel Tournament = new();
  private List<MatchUpModel> MatchUps = new();

  [Parameter]
  public EventCallback<TournamentModel> TournamentFormat { get; set; }

  private void UpdateTeamName(ChangeEventArgs e, TeamModel team)
  {
    team.TeamName = e.Value.ToString();
    TournamentFormat.InvokeAsync(Tournament);
  }

  private void AddTeam()
  {
    TeamModel team = new();
    int teamCount = Teams.Count + 1;
    team.TeamName = $"Team {teamCount}";
    Teams.Add(team);
    if (teamCount < 2)
    {
      return;
    }

    GetRoundsAndPositions(teamCount);
    AddingAllMatchups();
    PlacingTeamsInThereMatchUps(teamCount);
    CheckingForFreeWins();

    Tournament.MatchUps = MatchUps.OrderByDescending(x => x.MatchPosition).ToList();

    TournamentFormat.InvokeAsync(Tournament);
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
        updateMatch.TeamOne = Teams[i];
        firstFirstRoundPosition--;
      }
      else
      {
        var updateMatch = MatchUps.First(x => x.MatchPosition == lastFirstRoundPosition);
        updateMatch.TeamTwo = Teams[i];
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

  private void RemoveTeam(TeamModel team)
  {
    Teams.Remove(team);

    int teamCount = Teams.Count;

    GetRoundsAndPositions(teamCount);
    AddingAllMatchups();
    PlacingTeamsInThereMatchUps(teamCount);
    CheckingForFreeWins();

    Tournament.MatchUps = MatchUps.OrderByDescending(x => x.MatchPosition).ToList();

    TournamentFormat.InvokeAsync(Tournament);
  }

  private async Task SaveData()
  {
    Tournament.User.Id = 1;
    await tournamentData.CreateTournament(Tournament, Teams);
  }
}