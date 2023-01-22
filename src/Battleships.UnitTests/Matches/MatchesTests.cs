using Battleships.Console.Fleets;
using Battleships.Console.Matches;
using FluentAssertions;

namespace Battleships.UnitTests.Matches;

public class MatchesTests
{
    [Fact]
    public void initial_state_allows_to_shoot_a_selected_target()
    {
        var fleet = Fleet.Create(FleetShip.Create(CoordinatesSet.Create((3, 3), (3, 4))));
        var match = new Match(fleet);

        var result = match.Handle(new ShootATarget((0, 0)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainEquivalentOf(new ShootMissedEvent((0, 0)));
    }
}