using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.Matches;

public interface IMatchCommand { }
public sealed record ShootATarget(Coordinates Coordinates) : IMatchCommand;