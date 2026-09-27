namespace Wordle.App;

public class SessionStatistics
{
    public int GamesPlayed { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int TotalAttempts { get; private set; }

    public double AverageAttempts =>
        GamesPlayed == 0 ? 0.0 : (double)TotalAttempts / GamesPlayed;

    public double WinsPercent =>
        GamesPlayed == 0.0 ? 0.0 : (double)Wins / GamesPlayed * 100;

    public void AddGame(GameSession session)
    {
        GamesPlayed++;
        TotalAttempts += session.UsedAttempts;

        switch (session.Status)
        {
            case GameStatus.Win:
                Wins++;
                break;

            case GameStatus.Lose:
                Losses++;
                break;
        }
    }
}