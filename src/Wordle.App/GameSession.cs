namespace Wordle.App;

public class GameSession
{
    public readonly int MaxAttempts;
    public readonly string CorrectAnswer;
    public int UsedAttempts { get; private set; }
    public GameStatus Status { get; private set; }
    public IReadOnlyList<string> UserAnswerHistory => _userAnswerHistory;
    private readonly List<string> _userAnswerHistory = new();

    public GameSession(int maxAttempts, string correctAnswer)
    {
        if (maxAttempts <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxAttempts),
                maxAttempts,
                "MaxAttempts must be greater than zero");
        }

        if (string.IsNullOrEmpty(correctAnswer))
        {
            throw new ArgumentException(
                "Correct answer cannot be empty",
                nameof(correctAnswer));
        }

        MaxAttempts = maxAttempts;
        Status = GameStatus.InProgress;
        UsedAttempts = 0;
        CorrectAnswer = correctAnswer;
    }

    public void ApplyValidGuess(string guess)
    {
        UsedAttempts++;
        _userAnswerHistory.Add(guess);

        if (guess == CorrectAnswer)
        {
            Status = GameStatus.Win;
            return;
        }

        if (UsedAttempts == MaxAttempts)
        {
            Status = GameStatus.Lose;
        }
    }
}