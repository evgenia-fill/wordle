namespace Wordle.App;

public static class ReadDictionary
{
    public static List<string> Read()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "dictionary.txt");
        var words = File.ReadAllLines(path);
        return words.Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.ToLower())
            .ToList();
    }
}