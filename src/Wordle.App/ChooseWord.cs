namespace Wordle.App;

public static class ChooseWord
{
    private static readonly List<string> Words = ReadDictionary.Read();

    public static string Choose(int seed)
    {
        var random = new Random(seed);
        return Words[random.Next(Words.Count)];
    }
}