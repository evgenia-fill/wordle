namespace Wordle.App;

public class ConsoleGame
{
    private readonly GameEngine _engine;

    public ConsoleGame(GameEngine engine)
    {
        _engine = engine;
    }

    public void Play(GameSession session)
    {
        while (session.Status == GameStatus.InProgress)
        {
            Console.Write("Ваша попытка: ");
            var guess = Console.ReadLine();

            var guessResult = _engine.ApplyGuess(session, guess);

            switch (guessResult.Type)
            {
               case GuessResultType.LengthError:
                    Console.WriteLine($"Слово \"{guess}\" должно быть длиной {GameEngine.RequiredWordLength}!");
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    break;

                case GuessResultType.NotInDictionaryError:
                    Console.WriteLine($"Слова \"{guess}\" нет в словаре!");
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    break;

                case GuessResultType.Valid:
                    ShowResult(guessResult.LetterStatuses);
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    break;
            }

            Console.WriteLine();

            if (session.Status == GameStatus.Win)
            {
                Console.WriteLine("Поздравляю! Вы выиграли!");
                break;
            }

            if (session.Status == GameStatus.Lose)
            {
                Console.WriteLine("Поражение!");
                Console.WriteLine($"Загаданное слово: {session.CorrectAnswer}");
                break;
            }
        }
    }

    private static void ShowResult(IReadOnlyList<LetterStatus>? statuses)
    {
        if (statuses == null)
        {
            return;
        }

        foreach (var letterStatus in statuses)
        {
            switch (letterStatus)
            {
                case LetterStatus.OnCorrectPlace:
                    Console.Write('✅');
                    break;
                case LetterStatus.OnWrongPlace:
                    Console.Write("🟡");
                    break;
                case LetterStatus.NotContained:
                    Console.Write('❌');
                    break;
            }
        }
    }
}