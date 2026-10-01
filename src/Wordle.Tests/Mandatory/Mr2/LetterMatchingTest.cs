using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: раскраска букв.</summary>
public class LetterMatchingTest
{
    [Fact(DisplayName = "Базовый случай: загадано \"озеро\", ввод \"арбуз\" -> ❌🟡❌❌🟡")]
    public void BasicCase()
    {
        const string correctAnswer = "озеро";
        const string guess = "арбуз";
        var expectedAnswer = new[] { LetterStatus.NotContained, LetterStatus.OnWrongPlace, LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.OnWrongPlace };
        Assert.Equal(expectedAnswer, GuessChecker.Check(guess, correctAnswer));
    }

    [Fact(DisplayName = "Полное совпадение: загадано \"озеро\", ввод \"озеро\" -> ✅✅✅✅✅")]
    public void ExactMatch()
    {
        const string correctAnswer = "озеро";
        const string guess = "озеро";
        var expectedAnswer = new[] { LetterStatus.OnCorrectPlace, LetterStatus.OnCorrectPlace, LetterStatus.OnCorrectPlace, LetterStatus.OnCorrectPlace, LetterStatus.OnCorrectPlace };
        Assert.Equal(expectedAnswer, GuessChecker.Check(guess, correctAnswer));
    }

    [Fact(DisplayName = "Повторяющиеся буквы: загадано \"сорок\", ввод \"оооом\" -> ❌✅❌✅❌")]
    public void RepeatedLettersAreNotDoubleCounted()
    {
        const string correctAnswer = "сорок";
        const string guess = "оооом";
        var expectedAnswer = new[] { LetterStatus.NotContained, LetterStatus.OnCorrectPlace, LetterStatus.NotContained, LetterStatus.OnCorrectPlace, LetterStatus.NotContained };
        Assert.Equal(expectedAnswer, GuessChecker.Check(guess, correctAnswer));
    }

    [Fact(DisplayName = "Ни одна буква не подошла: все позиции ❌")]
    public void NoMatchingLetters()
    {
        const string correctAnswer = "сорок";
        const string guess = "ййййй";
        var expectedAnswer = new[] { LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.NotContained };
        Assert.Equal(expectedAnswer, GuessChecker.Check(guess, correctAnswer));
    }
}