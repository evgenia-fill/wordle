using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: валидация ввода.</summary>
public class GuessValidationTest
{
    private const int MaxAttempts = 3;
    private const string CorrectWord = "кашка";

    [Theory(DisplayName = "Слово не из 5 букв отклоняется")]
    [InlineData("дом")]
    [InlineData("домики")]
    [InlineData("")]
    public void WordOfWrongLengthIsRejected(string guess)
    {
        var dictionary = new List<string>(4) { "кашка", "сашка", "машка", "мишка" };

        var session = new GameSession(MaxAttempts, CorrectWord);
        var engine = new GameEngine(dictionary);

        var guessResult = engine.ApplyGuess(session, guess);

        Assert.Equal(GuessResultType.LengthError, guessResult.Type);
    }

    [Theory(DisplayName = "Ввод с не-буквами отклоняется")]
    [InlineData("дом12")]
    [InlineData("дом!!")]
    [InlineData("до ма")]
    public void NonLetterInputIsRejected(string guess)
    {
        var dictionary = new List<string>(4) { "кашка", "сашка", "машка", "мишка" };

        var session = new GameSession(MaxAttempts, CorrectWord);
        var engine = new GameEngine(dictionary);

        var guessResult = engine.ApplyGuess(session, guess);

        Assert.Equal(GuessResultType.LetterError, guessResult.Type);
    }

    [Fact(DisplayName = "Слово, которого нет в словаре, отклоняется")]
    public void WordOutsideDictionaryIsRejected()
    {
        const string guess = "берег";
        var dictionary = new List<string>(4) { "кашка", "сашка", "машка", "мишка" };

        var session = new GameSession(MaxAttempts, CorrectWord);
        var engine = new GameEngine(dictionary);

        var guessResult = engine.ApplyGuess(session, guess);

        Assert.Equal(GuessResultType.NotInDictionaryError, guessResult.Type);
    }

    [Fact(DisplayName = "Некорректный ввод не тратит попытку")]
    public void InvalidInputDoesNotConsumeAttempt()
    {
        const string notInDictionaryGuess = "берег";
        const string letterErrorGuess = "дом!!";
        const string lengthErrorGuess = "дом";

        var dictionary = new List<string>(4) { "кашка", "сашка", "машка", "мишка" };

        var session = new GameSession(MaxAttempts, CorrectWord);
        var engine = new GameEngine(dictionary);

        engine.ApplyGuess(session, notInDictionaryGuess);
        engine.ApplyGuess(session, letterErrorGuess);
        engine.ApplyGuess(session, lengthErrorGuess);

        var freeAttempts = session.MaxAttempts - session.UsedAttempts;

        Assert.Equal(MaxAttempts, freeAttempts);
    }

    [Fact(DisplayName = "Ввод не зависит от регистра: \"ОЗЕРО\" и \"озеро\" обрабатываются одинаково")]
    public void InputIsCaseInsensitive()
    {
        const string upperCaseGuess = "ОЗЕРО";
        const string lowerCaseGuess = "озеро";

        var dictionary = new List<string>(4) { "кашка", "озеро", "машка", "мишка" };

        var engine = new GameEngine(dictionary);

        var upperCaseSession = new GameSession(MaxAttempts, CorrectWord);
        var lowerCaseSession = new GameSession(MaxAttempts, CorrectWord);

        var upperCaseResult = engine.ApplyGuess(upperCaseSession, upperCaseGuess);
        var lowerCaseResult = engine.ApplyGuess(lowerCaseSession, lowerCaseGuess);

        Assert.Equal(upperCaseResult.Type, lowerCaseResult.Type);
        Assert.Equal(upperCaseResult.LetterStatuses, lowerCaseResult.LetterStatuses);
    }
}