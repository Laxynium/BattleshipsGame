namespace Battleships.Console.MatchCockpit;

public record MatchViewModel
{
    public MatchCockpitViewModel Cockpit { get; set; }
    public string State { get; set; } = "waiting_for_game";
}