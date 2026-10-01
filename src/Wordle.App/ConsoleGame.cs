using System.Text;

namespace Wordle.App;

public class ConsoleGame
{
    private readonly GameEngine _engine;

    public ConsoleGame(GameEngine engine)
    {
        _engine = engine;
    }

    public ConsoleGameResult Play(GameSession session, SessionStatistics statistics)
    {
        while (session.Status == GameStatus.InProgress)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Ваша попытка: ");
            Console.ResetColor();
            var guess = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (guess)
            {
                case "\\h":
                    ShowCommands();
                    Console.WriteLine();
                    continue;

                case "\\r":
                    ShowRules();
                    Console.WriteLine();
                    continue;

                case "\\q":
                    return ConsoleGameResult.Quit;

                case "\\s":
                    return ConsoleGameResult.StartNewGame;
                
                case "\\stat":
                    ShowStatistics(statistics);
                    Console.WriteLine();
                    break;
            }
            
            if (guess?.StartsWith('\\') == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
                Console.ResetColor();
                continue;
            }

            var guessResult = _engine.ApplyGuess(session, guess);

            switch (guessResult.Type)
            {
                case GuessResultType.LengthError:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Слово \"{guess}\" должно быть длиной {GameRules.RequiredWordLength}!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    Console.ResetColor();
                    break;

                case GuessResultType.LetterError:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Слово \"{guess}\" должно состоять только из букв!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    Console.ResetColor();
                    break;

                case GuessResultType.NotInDictionaryError:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Слова \"{guess}\" нет в словаре!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    Console.ResetColor();
                    break;

                case GuessResultType.Valid:
                    ShowResult(guessResult.LetterStatuses);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine();

            switch (session.Status)
            {
                case GameStatus.Win:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Поздравляю! Вы выиграли!");
                    Console.ResetColor();
                    return ConsoleGameResult.Finished;
                case GameStatus.Lose:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Поражение!");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Загаданное слово: {session.CorrectAnswer}");
                    Console.ResetColor();
                    return ConsoleGameResult.Finished;
            }
        }

        return ConsoleGameResult.Finished;
    }

    public static void ShowCommands()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("""
                          Доступные команды:

                          \h — посмотреть список команд
                          \r — посмотреть правила игры
                          \stat — посмотреть статистику
                          \s — начать новую игру
                          \q — выйти
                          """);
        Console.ResetColor();
    }

    public static void ShowRules()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(new StringBuilder()
            .Append("""
                    Правила игры

                    Программа выбирает слово из словаря, а Вы пытаетесь его угадать
                    за ограниченное количество попыток.

                    После каждой валидной попытки Вы получаете подсказку по каждой букве:

                    ✅ — буква стоит на правильном месте;
                    🟡 — буква есть в загаданном слове, но стоит на другом месте;
                    ❌ — такой буквы среди оставшихся букв загаданного слова нет.

                    Пример:

                    Загадано: озеро
                    Ввод:     арбуз

                    Результат: ❌🟡❌❌🟡
                    """)
            .ToString());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Важно! Игра нечувствительна к регистрам, но чувствительна к букве \"ё\".");

        Console.ResetColor();
    }

    public static void ShowStatistics(SessionStatistics statistics)
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.WriteLine("""
                          Статистика сессии:
                          """);

        Console.WriteLine($"Сыграно игр: {statistics.GamesPlayed}");
        Console.WriteLine($"Побед: {statistics.Wins}");
        Console.WriteLine($"Поражений: {statistics.Losses}");
        Console.WriteLine($"Процент побед: {statistics.WinsPercent:F1}%");
        Console.WriteLine($"Среднее количество попыток: {statistics.AverageAttempts:F1}");

        Console.ResetColor();
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