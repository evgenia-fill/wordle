using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr2;

/// <summary>Обязательные тесты: завершение партии.</summary>
public class GameOutcomeTest
{
    [Fact(DisplayName = "Угаданное слово переводит сессию в статус WIN")]
    public void CorrectGuessWinsTheGame()
    {
        const int maxAttempts = 3;
        const string correctWord = "кашка";
        var session = new GameSession(maxAttempts, correctWord);
        session.ApplyValidGuess("кашка");
        Assert.Equal(GameStatus.Win, session.Status);
    }

    [Fact(DisplayName = "После 6 неудачных попыток сессия переходит в статус LOSE")]
    public void SixFailedAttemptsLoseTheGame()
    {
        const int maxAttempts = 6;
        const string correctWord = "кашка";
        var session = new GameSession(maxAttempts, correctWord);
        session.ApplyValidGuess("машка");
        session.ApplyValidGuess("макка");
        session.ApplyValidGuess("макка");
        session.ApplyValidGuess("котик");
        session.ApplyValidGuess("шалаш");
        session.ApplyValidGuess("берег");
        Assert.Equal(GameStatus.Lose, session.Status);
    }

    [Fact(DisplayName = "При поражении показывается загаданное слово")]
    public void AnswerIsRevealedOnLoss()
    {
        const int maxAttempts = 3;
        const string correctWord = "кашка";
        var dictionary = new List<string>(4) { "кашка", "сашка", "машка", "мишка" };

        var engine = new GameEngine(dictionary);
        var session = new GameSession(maxAttempts, correctWord);
        var consoleGame = new ConsoleGame(engine);

        var statistics = new SessionStatistics();

        var input = new StringReader(
            "сашка\n" +
            "машка\n" +
            "мишка\n");

        var output = new StringWriter();

        var originalIn = Console.In;
        var originalOut = Console.Out;

        try
        {
            Console.SetIn(input);
            Console.SetOut(output);

            consoleGame.Play(session, statistics);

            Assert.Equal(GameStatus.Lose, session.Status);
            Assert.Contains($"Загаданное слово: {correctWord}", output.ToString());
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }

    [Fact(DisplayName = "Завершённая партия больше не принимает попытки")]
    public void FinishedGameRejectsFurtherGuesses()
    {
        const int maxAttempts = 3;
        const string correctWord = "кашка";
        var dictionary = new List<string>(4) { "кашка", "сашка", "машка", "мишка" };

        var session = new GameSession(maxAttempts, correctWord);
        var engine = new GameEngine(dictionary);

        engine.ApplyGuess(session, correctWord);
        Assert.Equal(GameStatus.Win, session.Status);

        var result = engine.ApplyGuess(session, correctWord);
        Assert.Equal(GuessResultType.GameFinishedError, result.Type);
    }
}