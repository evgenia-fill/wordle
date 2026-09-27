namespace Wordle.App;

public class GameEngine
{
    private readonly IReadOnlyList<string> _dictionary;

    public GameEngine(IReadOnlyList<string> dictionary)
    {
        _dictionary = dictionary;
    }

    public GuessResult ApplyGuess(GameSession session, string? guess)
    {
        guess = guess?.ToLower();

        if (session.Status != GameStatus.InProgress)
        {
            return new GuessResult(GuessResultType.GameFinishedError);
        }

        if (string.IsNullOrWhiteSpace(guess) || guess.Length != GameRules.RequiredWordLength)
        {
            return new GuessResult(GuessResultType.LengthError);
        }

        if (!guess.All(char.IsLetter))
        {
            return new GuessResult(GuessResultType.LetterError);
        }

        if (!_dictionary.Contains(guess))
        {
            return new GuessResult(GuessResultType.NotInDictionaryError);
        }

        session.ApplyValidGuess(guess);
        var statuses = GuessChecker.Evaluate(guess, session.CorrectAnswer);

        return new GuessResult(GuessResultType.Valid, statuses);
    }
}