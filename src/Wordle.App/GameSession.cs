namespace Wordle.App;

public class GameSession
{
    public int MaxAttempts { get; private set; }
    public int UsedAttempts { get; set; }
    public GameStatus Status { get; private set; }
    private string CorrectAnswer { get; set; }
    public List<string> UserAnswerHistory { get; private set; }

    public GameSession(int maxAttempts, string correctAnswer)
    {
        if (maxAttempts <= 0)
        {
            throw new ArgumentOutOfRangeException("MaxAttempts must be greater than zero", nameof(maxAttempts));
        }

        MaxAttempts = maxAttempts;
        Status = GameStatus.InProgress;
        UsedAttempts = 0;
        UserAnswerHistory = new List<string>();
        CorrectAnswer = correctAnswer;
    }

    public void MakeGuess(string guess)
    {
        if (Status != GameStatus.InProgress)
        {
            throw new Exception("Game is finished");
        }

        if (guess.Length != 5)
        {
            throw new ArgumentException("Word must be 5 characters length", nameof(guess.Length));
        }

        UsedAttempts++;
        UserAnswerHistory.Add(guess);

        if (guess == CorrectAnswer)
        {
            Status = GameStatus.Win;
            return;
        }

        if (UsedAttempts == MaxAttempts)
        {
            Status = GameStatus.Lose;
            return;
        }
    }
}