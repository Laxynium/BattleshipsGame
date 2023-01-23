namespace Battleships.Console.Matches;

public class MatchRepository
{
    private readonly Dictionary<string, Match> _matches = new();
    
    public void Save(Match match)
    {
        _matches[match.Id] = match;
    }

    public Match? Load(string id)
    {
        if (!_matches.ContainsKey(id))
        {
            return null;
        }
        
        return _matches[id];
    }
}