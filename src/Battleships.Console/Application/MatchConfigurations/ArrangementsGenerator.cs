using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.MatchConfigurations;

public class ArrangementsGenerator
{
    public static IReadOnlyCollection<CoordinatesSet> GenerateUsingTranslationsFor(
        ShipBlueprint shipBlueprint, GridConstrains gridConstrains)
    {
        var normalized = NormalizeToOrigin(shipBlueprint.Set);

        var ((minX, maxX), (minY, maxY)) = normalized.GetBoundaries();
        
        var translations =
            GetTranslations(minX, maxX, gridConstrains.Width).SelectMany(
                    _ => GetTranslations(minY, maxY, gridConstrains.Height),
                    (xTranslation, yTranslation) => (x: xTranslation, y: yTranslation))
                .ToList();
            
        var translated = translations
            .Select(t => shipBlueprint.Set.Translate(t.x, t.y))
            .ToList();
        
        return translated;
    }

    public static IReadOnlyCollection<CoordinatesSet> GenerateUsingRotationsFor(
        ShipBlueprint shipBlueprint, GridConstrains gridConstrains)
    {
        var normalized = NormalizeToOrigin(shipBlueprint.Set);
        
        var rotated = new[]
            {
                CounterclockwiseRotation.Rotation0,
                CounterclockwiseRotation.Rotation90,
                CounterclockwiseRotation.Rotation180,
                CounterclockwiseRotation.Rotation270
            }
            .Select(x => normalized.Rotate(x))
            .Select(NormalizeToOrigin);

        return rotated
            .Where(x =>
            {
                var ((minX, maxX), (minY, maxY)) = x.GetBoundaries();

                return minX >= 0 && maxX < gridConstrains.Width
                                 && minY >= 0 && maxY < gridConstrains.Height;
                
            })
            .ToHashSet();
    }

    private static CoordinatesSet NormalizeToOrigin(CoordinatesSet set)
    {
        var ((minX,_), (minY, _)) = set.GetBoundaries();
        
        var normalized = set.Translate(-minX, -minY);
        
        return normalized;
    }

    private static List<int> GetTranslations(int minCoord, int maxCoord, int boundary) =>
        Enumerable.Range(0, minCoord).Select(i => minCoord - i)
            .Concat(Enumerable.Range(maxCoord, boundary-maxCoord).Select(i => i - maxCoord))
            .ToList();
    
}