namespace Wordle.App;

public static class WordleApplication
{
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Добро пожаловать в Wordle!");
        Console.ResetColor();
        Thread.Sleep(1000);

        ConsoleGame.ShowCommands();

        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Введите команду >> ");
            Console.ResetColor();

            var command = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (command)
            {
                case "\\h":
                    ConsoleGame.ShowCommands();
                    break;

                case "\\r":
                    ConsoleGame.ShowRules();
                    break;

                case "\\q":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("До встречи!");
                    Console.ResetColor();
                    return;

                case "\\s":
                    var result = StartGame();

                    if (result == ConsoleGameResult.Quit)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("До встречи!");
                        Console.ResetColor();
                        return;
                    }

                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
                    Console.ResetColor();
                    break;
            }
        }


        static ConsoleGameResult StartGame()
        {
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Выберите режим игры:");
                Console.WriteLine("[1] Обычная игра (seed выбирается автоматически)");
                Console.WriteLine("[2] Игра с заданным seed (seed задаёте Вы)");
                Console.ResetColor();

                var (seed, seedCommand) = ReadSeed();

                switch (seedCommand)
                {
                    case ConsoleGameResult.Quit:
                        return ConsoleGameResult.Quit;
                    case ConsoleGameResult.StartNewGame:
                        Console.Clear();
                        continue;
                }

                var (maxAttempts, attemptsCommand) = ReadMaxAttempts();

                switch (attemptsCommand)
                {
                    case ConsoleGameResult.Quit:
                        return ConsoleGameResult.Quit;
                    case ConsoleGameResult.StartNewGame:
                        Console.Clear();
                        continue;
                }

                const string fileName = "dictionary.txt";

                var dictionary = WordleDictionary.LoadFromFile(fileName);
                var correctWord = WordSelector.Choose(seed!.Value, dictionary);
                var engine = new GameEngine(dictionary);

                var session = new GameSession(maxAttempts!.Value, correctWord);
                var consoleGame = new ConsoleGame(engine);

                var consoleGameResult = consoleGame.Play(session);

                switch (consoleGameResult)
                {
                    case ConsoleGameResult.StartNewGame:
                        Console.Clear();
                        continue;

                    case ConsoleGameResult.Quit:
                        return ConsoleGameResult.Quit;
                }

                var (isShowHistory, historyCommand) = AskYesNo("Хотите посмотреть свои попытки?");
                switch (historyCommand)
                {
                    case ConsoleGameResult.Quit:
                        return ConsoleGameResult.Quit;
                    case ConsoleGameResult.StartNewGame:
                        Console.Clear();
                        continue;
                }

                if (isShowHistory == true)
                {
                    ShowUserAnswerHistory(session);
                }

                var (isPlayingAgain, playingCommand) = AskYesNo("Хотите сыграть ещё раз?");
                switch (playingCommand)
                {
                    case ConsoleGameResult.Quit:
                        return ConsoleGameResult.Quit;
                    case ConsoleGameResult.StartNewGame:
                        Console.Clear();
                        continue;
                }

                if (isPlayingAgain == true)
                {
                    Console.Clear();
                    continue;
                }

                return ConsoleGameResult.Quit;
            }
        }
    }

    private static (int? Seed, ConsoleGameResult? Command) ReadSeed()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(">> ");
            Console.ResetColor();

            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (TryHandleServiceCommand(input, out var command))
            {
                if (command != null)
                {
                    return (null, command);
                }

                continue;
            }

            switch (input)
            {
                case "1":
                    return (Random.Shared.Next(), null);

                case "2":
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("Введите seed, по которому выбирается слово из словаря >> ");
                        Console.ResetColor();

                        var seedInput = Console.ReadLine()?.Trim().ToLowerInvariant();

                        if (TryHandleServiceCommand(seedInput, out command))
                        {
                            if (command != null)
                            {
                                return (null, command);
                            }

                            continue;
                        }

                        if (seedInput?.StartsWith('\\') == true)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
                            Console.ResetColor();
                            continue;
                        }

                        if (int.TryParse(seedInput, out var seed))
                        {
                            return (seed, null);
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Seed должен быть целым числом");
                        Console.ResetColor();
                    }

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Такого режима игры нет!");
                    Console.ResetColor();
                    break;
            }
        }
    }


    private static (int? MaxAttempts, ConsoleGameResult? Command) ReadMaxAttempts()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Введите максимальное количество попыток >> ");
            Console.ResetColor();

            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (TryHandleServiceCommand(input, out var command))
            {
                if (command != null)
                {
                    return (null, command);
                }

                continue;
            }

            if (input?.StartsWith('\\') == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
                Console.ResetColor();
                continue;
            }

            if (int.TryParse(input, out var maxAttempts) && maxAttempts > 0)
            {
                return (maxAttempts, null);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Количество попыток должно быть целым числом больше 0");
            Console.ResetColor();
        }
    }


    private static bool TryHandleServiceCommand(string? input, out ConsoleGameResult? result)
    {
        result = null;

        switch (input)
        {
            case "\\h":
                ConsoleGame.ShowCommands();
                return true;

            case "\\r":
                ConsoleGame.ShowRules();
                return true;

            case "\\q":
                result = ConsoleGameResult.Quit;
                return true;

            case "\\s":
                result = ConsoleGameResult.StartNewGame;
                return true;

            default:
                return false;
        }
    }

    private static (bool? Answer, ConsoleGameResult? Command) AskYesNo(string question)
    {
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(question);
            Console.WriteLine("[Д] Да");
            Console.WriteLine("[Н] Нет");
            Console.Write(">> ");
            Console.ResetColor();

            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (TryHandleServiceCommand(input, out var command))
            {
                if (command != null)
                {
                    return (null, command);
                }

                continue;
            }

            if (input?.StartsWith('\\') == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
                Console.ResetColor();
                continue;
            }

            switch (input)
            {
                case "д":
                    return (true, null);

                case "н":
                    return (false, null);

                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Введите \"Д\" или \"Н\"");
                    Console.ResetColor();
                    break;
            }
        }
    }

    private static void ShowUserAnswerHistory(GameSession session)
    {
        var userAnswerHistory = session.UserAnswerHistory;

        if (userAnswerHistory.Count != 0)

        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("История ответов:");
            Console.ResetColor();

            foreach (var userAnswer in userAnswerHistory)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"- {userAnswer}");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("История ответов пуста!");
            Console.ResetColor();
        }
    }
}