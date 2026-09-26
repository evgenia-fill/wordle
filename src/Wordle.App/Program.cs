using Wordle.App;

Console.WriteLine("Добро пожаловать в Wordle!");
Thread.Sleep(1000);

ShowCommands();

while (true)
{
    Console.WriteLine();
    Console.Write("Введите команду >> ");

    var command = Console.ReadLine()?.Trim().ToLowerInvariant();

    switch (command)
    {
        case "\\h":
            ShowCommands();
            break;

        case "\\r":
            ShowRules();
            break;

        case "\\q":
            Console.WriteLine("До встречи!");
            return;

        case "\\s":
            StartGame();
            break;

        default:
            Console.WriteLine("Неизвестная команда. Введите \\h, чтобы посмотреть список команд");
            break;
    }
}

static void StartGame()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Выберите режим игры:");
        Console.WriteLine("[1] Обычная игра (seed выбирается автоматически)");
        Console.WriteLine("[2] Игра с заданным seed (seed задаёте Вы)");

        var seed = ReadSeed();
        var maxAttempts = ReadMaxAttempts();

        const string fileName = "dictionary.txt";

        var dictionary = WordleDictionary.LoadFromFile(fileName);
        var correctWord = WordSelector.Choose(seed, dictionary);
        var engine = new GameEngine(dictionary);

        var session = new GameSession(maxAttempts, correctWord);
        var consoleGame = new ConsoleGame(engine);

        consoleGame.Play(session);

        Console.WriteLine();
        Console.WriteLine("Хотите сыграть ещё раз?");
        Console.WriteLine("[Д] Да");
        Console.WriteLine("[Н] Нет");

        var restart = Console.ReadKey(intercept: true);
        Console.WriteLine();

        if (char.ToUpperInvariant(restart.KeyChar) != 'Д')
        {
            Console.Clear();
            return;
        }

        Console.Clear();
    }
}

static int ReadSeed()
{
    while (true)
    {
        var key = Console.ReadKey(intercept: true);
        Console.WriteLine();

        switch (key.KeyChar)
        {
            case '1':
                return Random.Shared.Next();

            case '2':
                Console.Write("Введите seed, по которому выбирается слово из словаря >> ");

                if (int.TryParse(Console.ReadLine(), out var seed))
                {
                    return seed;
                }

                Console.WriteLine("Seed должен быть целым числом");
                break;

            default:
                Console.WriteLine("Такого режима игры нет!");
                break;
        }
    }
}

static int ReadMaxAttempts()
{
    while (true)
    {
        Console.Write("Введите максимальное количество попыток >> ");

        if (int.TryParse(Console.ReadLine(), out var maxAttempts) &&
            maxAttempts > 0)
        {
            return maxAttempts;
        }

        Console.WriteLine("Количество попыток должно быть целым числом больше 0");
    }
}

static void ShowCommands()
{
    Console.WriteLine("""
                      Доступные команды:

                      \h — посмотреть список команд
                      \r — посмотреть правила игры
                      \s — начать новую игру
                      \q — выйти
                      """);
}

static void ShowRules()
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