﻿using Battleships.Console;
using Battleships.Console.Application;
using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.MatchConfigurations;
using FluentAssertions;

namespace Battleships.UnitTests.MatchConfigurations;

public class CreatingFleetFromMatchConfigurationTests
{
    [Fact]
    public void error_when_outside_of_grid_width()
    {
        var matchConfiguration = new MatchConfiguration(new GridConstrains(5, 5),
            ShipBlueprintsStock.Create(
                ("1", AShipBlueprint("----"))));

        var fixedFleetArranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"A3","A4","A5","A6"})
        });
        
        var action = () =>matchConfiguration.CreateFleet(fixedFleetArranger);

        action.Should().Throw<Exception>().WithMessage("*outside*");
    }

    [Fact]
    public void error_when_outside_of_grid_height()
    {
        var matchConfiguration = new MatchConfiguration(new GridConstrains(5, 5),
            ShipBlueprintsStock.Create(
                ("1", AShipBlueprint("----"))));

        var fixedFleetArranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"D3","E3","F3","G3"})
        });
        
        var action = () =>matchConfiguration.CreateFleet(fixedFleetArranger);

        action.Should().Throw<Exception>().WithMessage("*outside*");
    }

    [Fact]
    public void error_when_some_ship_is_not_arranged()
    {
        var matchConfiguration = new MatchConfiguration(new GridConstrains(5, 5),
            ShipBlueprintsStock.Create(
                ("1", AShipBlueprint("----")),
                ("2", AShipBlueprint("--"))));

        var fixedFleetArranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"A3","B3","C3","D3"})
        });
        
        var action = () =>matchConfiguration.CreateFleet(fixedFleetArranger);

        action.Should().Throw<Exception>().WithMessage("*does not match*");
    }

    [Fact]
    public void error_when_there_is_some_extra_ship()
    {
        var matchConfiguration = new MatchConfiguration(new GridConstrains(5, 5),
            ShipBlueprintsStock.Create(
                ("1", AShipBlueprint("----"))));

        var fixedFleetArranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"A3","B3","C3","D3"}),
            (new FleetShipId("2"), new GridCoordinates[]{"A4","B4"}),
        });
        
        var action = () =>matchConfiguration.CreateFleet(fixedFleetArranger);

        action.Should().Throw<Exception>().WithMessage("*does not match*");
    }

    [Fact]
    public void error_when_there_are_overlapping_ships()
    {
        var matchConfiguration = new MatchConfiguration(new GridConstrains(5, 5),
            ShipBlueprintsStock.Create(
                ("1", AShipBlueprint("----"))));

        var fixedFleetArranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"A3","B3","C3","D3"}),
            (new FleetShipId("2"), new GridCoordinates[]{"B3","B4"}),
        });
        
        var action = () =>matchConfiguration.CreateFleet(fixedFleetArranger);

        action.Should().Throw<Exception>().WithMessage("*overlapping*");
    }

    [Fact]
    public void error_when_ship_size_does_not_match_one_specified_in_blueprint()
    {
        var matchConfiguration = new MatchConfiguration(new GridConstrains(5, 5),
            ShipBlueprintsStock.Create(
                ("1", AShipBlueprint("----"))));

        var fixedFleetArranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"A3","B3","C3","D3","E3"})
        });
        
        var action = () =>matchConfiguration.CreateFleet(fixedFleetArranger);

        action.Should().Throw<Exception>().WithMessage("*size does not match*");
    }

    private static ShipBlueprint AShipBlueprint(string text) => 
         ShipBlueprint.FromText("not-relevant",text);
}