namespace Wordle.App;

public static class ChooseWord
{
    public static string Choose(int seed, List<string> dictionary)
    {
        var random = new Random(seed);
        return dictionary[random.Next(dictionary.Count)];
    }
}