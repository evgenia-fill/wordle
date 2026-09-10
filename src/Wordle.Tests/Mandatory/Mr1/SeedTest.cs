using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: одинаковый seed обязан давать одинаковое загаданное слово.</summary>
public class SeedTest
{
    private readonly List<string> _dictionary = GetDictionary.Get("dictionary.txt");

    [Fact(DisplayName = "Одинаковый seed даёт одинаковое загаданное слово")]
    public void SameSeedProducesSameAnswer()
    {
        const int seed = 67;
        var firstWord = ChooseWord.Choose(seed, _dictionary);
        var secondWord = ChooseWord.Choose(seed, _dictionary);
        Assert.Equal(firstWord, secondWord);
    }

    [Fact(DisplayName = "Разные seed'ы дают разные слова хотя бы иногда")]
    public void DifferentSeedsProduceDifferentAnswers()
    {
        var firstSeed = 6767;
        var secondSeed = 67;
        var firstWord = ChooseWord.Choose(firstSeed, _dictionary);
        var secondWord = ChooseWord.Choose(secondSeed, _dictionary);
        Assert.NotEqual(firstWord, secondWord);
    }
}