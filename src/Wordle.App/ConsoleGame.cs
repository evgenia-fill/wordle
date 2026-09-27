namespace Wordle.App;

public class ConsoleGame
{
    private readonly GameEngine _engine;

    public ConsoleGame(GameEngine engine)
    {
        _engine = engine;
    }

    public ConsoleGameResult Play(GameSession session)
    {
        while (session.Status == GameStatus.InProgress)
        {
            Console.Write("Ваша попытка: ");
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
            }

            var guessResult = _engine.ApplyGuess(session, guess);

            switch (guessResult.Type)
            {
                case GuessResultType.LengthError:
                    Console.WriteLine($"Слово \"{guess}\" должно быть длиной {GameRules.RequiredWordLength}!");
                    Console.WriteLine($"Осталось попыток: {session.MaxAttempts - session.UsedAttempts}");
                    break;

                case GuessResultType.LetterError:
                    Console.WriteLine($"Слово \"{guess}\" должно состоять только из букв!");
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

            switch (session.Status)
            {
                case GameStatus.Win:
                    Console.WriteLine("Поздравляю! Вы выиграли!");
                    return ConsoleGameResult.Finished;
                case GameStatus.Lose:
                    Console.WriteLine("Поражение!");
                    Console.WriteLine($"Загаданное слово: {session.CorrectAnswer}");
                    return ConsoleGameResult.Finished;
            }
        }

        return ConsoleGameResult.Finished;
    }

    public static void ShowCommands()
    {
        Console.WriteLine("""
                          Доступные команды:

                          \h — посмотреть список команд
                          \r — посмотреть правила игры
                          \s — начать новую игру
                          \q — выйти
                          """);
    }

    public static void ShowRules()
    {
        Console.WriteLine("""
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

                          Важно! Игра нечувствительна к регистрам, но чувствительна к букве "ё".
                          """);
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