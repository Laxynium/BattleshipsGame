using Battleships.Console.Matches;
using FluentAssertions;

namespace Battleships.UnitTests.Matches;

public class MatchesTests
{
    [Fact]
    public void initial_state_allows_to_shoot_a_selected_target()
    {
        var match = new Match();

        var result = match.Handle(new ShootATarget((0, 0)));

        result.IsSuccess.Should().BeTrue();
    }
}