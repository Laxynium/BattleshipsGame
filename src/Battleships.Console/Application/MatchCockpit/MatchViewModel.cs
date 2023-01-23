namespace Battleships.Console.Application.MatchCockpit;

public record MatchConfigurationDto(Dictionary<string, string> ShipsNames);
public record MatchViewModel(MatchCockpitViewModel Cockpit, MatchStateDto State, MatchConfigurationDto Configuration);