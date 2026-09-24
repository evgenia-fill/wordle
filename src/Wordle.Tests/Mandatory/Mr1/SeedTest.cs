using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: одинаковый seed обязан давать одинаковое загаданное слово.</summary>
public class SeedTest
{
    private readonly IReadOnlyList<string> _dictionary = WordleDictionary.LoadFromFile("dictionary.txt");

    [Fact(DisplayName = "Одинаковый seed даёт одинаковое загаданное слово")]
    public void SameSeedProducesSameAnswer()
    {
        const int seed = 67;
        var firstWord = WordSelector.Choose(seed, _dictionary);
        var secondWord = WordSelector.Choose(seed, _dictionary);
        Assert.Equal(firstWord, secondWord);
    }

    [Fact(DisplayName = "Разные seed'ы дают разные слова хотя бы иногда")]
    public void DifferentSeedsProduceDifferentAnswers()
    {
        const int firstSeed = 6767;
        const int secondSeed = 67;
        var firstWord = WordSelector.Choose(firstSeed, _dictionary);
        var secondWord = WordSelector.Choose(secondSeed, _dictionary);
        Assert.NotEqual(firstWord, secondWord);
    }
}