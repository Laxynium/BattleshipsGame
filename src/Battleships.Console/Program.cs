using Battleships.Console;
using Battleships.Console.Fleets;
using Battleships.Console.MatchConfigurations;

var matchConfiguration = new MatchConfiguration(
    new GridConstrains(10, 10), ShipBlueprintsStock.Create(
        ("1", ShipBlueprint.FromText("-----")),
        ("2", ShipBlueprint.FromText("----")),
        ("3", ShipBlueprint.FromText("----"))));
var fleetArranger = new FixedFleetArranger(new []
{
    (new FleetShipId("1"), new GridCoordinates[] { "A1", "A2", "A3", "A4", "A5" }),
    (new FleetShipId("2"), new GridCoordinates[] { "B1", "B2", "B3", "B4" }),
    (new FleetShipId("3"), new GridCoordinates[] { "C1", "C2", "C3", "C4" }),
});

var gameFacade = new GameFacade(matchConfiguration, fleetArranger);

while (true)
{
    gameFacade.StartANewMatch();

    while (gameFacade.GetGameState() != "match_over")
    {
        Console.WriteLine();
        var cockpit = gameFacade.GetMatchCockpit();
        foreach (var gridLine in cockpit.TargetGrid.ToTextRepresentation())
        {
            Console.WriteLine(gridLine);
        }
        Console.WriteLine();
        
        var coordsText = Console.ReadLine();
        try
        {
            gameFacade.ShootATarget(coordsText!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);   
        }
    }

    Console.WriteLine("Congratulations! You have won a battleship match");
}