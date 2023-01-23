using Battleships.Console.Application.Fleets;

namespace Battleships.UnitTests.Builders;

public static class CoordinatesSetBuilder
{
    public static CoordinatesSet CreateCoordinatesSet(Coordinates head, params Coordinates[] tail) => 
        CoordinatesSet.Create(head, tail);
}