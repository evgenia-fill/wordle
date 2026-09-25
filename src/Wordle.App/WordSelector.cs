namespace Wordle.App;

public static class WordSelector
{
    public static string Choose(int seed, List<string> dictionary)
    {
        var random = new Random(seed);
        return dictionary[random.Next(dictionary.Count)];
    }
}