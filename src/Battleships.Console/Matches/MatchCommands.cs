using Battleships.Console.Fleets;

namespace Battleships.Console.Matches;

public interface IMatchCommand { }
public sealed record ShootATarget(Coordinates Coordinates) : IMatchCommand;