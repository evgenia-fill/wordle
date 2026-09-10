namespace Wordle.App;

public static class GetDictionary
{
    public static List<string> Get(string filename)
    {
        return Parse(Read(filename));
    }

    private static string[] Read(string filename)
    {
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Dictionary file not found.");
        }

        var lines = File.ReadAllLines(path);
        return lines;
    }

    private static List<string> Parse(IEnumerable<string> lines)
    {
        var words = lines.Where(l => !string.IsNullOrWhiteSpace(l) && l.Length == 5)
            .Select(l => l.ToLower())
            .ToList();

        return words.Count == 0 ? throw new InvalidOperationException("Dictionary cannot be empty.") : words;
    }
}