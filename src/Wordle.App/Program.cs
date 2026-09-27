using Wordle.App;

Console.WriteLine("Добро пожаловать в Wordle!");
Thread.Sleep(1000);

ConsoleGame.ShowCommands();

while (true)
{
    Console.WriteLine();
    Console.Write("Введите команду >> ");

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
            Console.WriteLine("До встречи!");
            return;

        case "\\s":
            var result = StartGame();

            if (result == ConsoleGameResult.Quit)
            {
                Console.WriteLine("До встречи!");
                return;
            }

            break;

        default:
            Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
            break;
    }
}


static ConsoleGameResult StartGame()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Выберите режим игры:");
        Console.WriteLine("[1] Обычная игра (seed выбирается автоматически)");
        Console.WriteLine("[2] Игра с заданным seed (seed задаёте Вы)");

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

            case ConsoleGameResult.Finished:
                break;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Хотите сыграть ещё раз?");
            Console.WriteLine("[Д] Да");
            Console.WriteLine("[Н] Нет");
            Console.Write(">> ");

            var inputKey = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (TryHandleServiceCommand(inputKey, out var commandResult))
            {
                if (commandResult == ConsoleGameResult.Quit)
                {
                    return ConsoleGameResult.Quit;
                }

                if (commandResult == ConsoleGameResult.StartNewGame)
                {
                    Console.Clear();
                    break;
                }

                continue;
            }

            switch (inputKey)
            {
                case "д":
                    Console.Clear();
                    break;

                case "н":
                    return ConsoleGameResult.Finished;

                default:
                    Console.WriteLine("Введите \"Д\" или \"Н\"");
                    continue;
            }

            break;
        }
    }
}


static (int? Seed, ConsoleGameResult? Command) ReadSeed()
{
    while (true)
    {
        Console.Write(">> ");

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
                    Console.Write("Введите seed, по которому выбирается слово из словаря >> ");

                    var seedInput = Console.ReadLine()?.Trim().ToLowerInvariant();

                    if (TryHandleServiceCommand(seedInput, out command))
                    {
                        if (command != null)
                        {
                            return (null, command);
                        }

                        continue;
                    }

                    if (int.TryParse(seedInput, out var seed))
                    {
                        return (seed, null);
                    }

                    Console.WriteLine("Seed должен быть целым числом");
                }

            default:
                Console.WriteLine("Такого режима игры нет!");
                break;
        }
    }
}


static (int? MaxAttempts, ConsoleGameResult? Command) ReadMaxAttempts()
{
    while (true)
    {
        Console.Write("Введите максимальное количество попыток >> ");

        var input = Console.ReadLine()?.Trim().ToLowerInvariant();

        if (TryHandleServiceCommand(input, out var command))
        {
            if (command != null)
            {
                return (null, command);
            }

            continue;
        }

        if (int.TryParse(input, out var maxAttempts) && maxAttempts > 0)
        {
            return (maxAttempts, null);
        }

        Console.WriteLine("Количество попыток должно быть целым числом больше 0");
    }
}


static bool TryHandleServiceCommand(
    string? input,
    out ConsoleGameResult? result)
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