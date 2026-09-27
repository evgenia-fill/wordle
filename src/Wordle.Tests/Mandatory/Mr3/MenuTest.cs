using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr3;

/// <summary>Обязательные тесты: меню и режим автопроверки.</summary>
public class MenuTest
{
    [Fact(DisplayName = "Некорректный пункт меню не роняет программу")]
    public void InvalidMenuChoiceDoesNotCrash()
    {
        var output = RunApplication(
            """
            abracadabra
            \q
            """);

        Assert.Contains("Неизвестная команда", output);
        Assert.Contains("До встречи!", output);
    }

    [Fact(DisplayName = "Можно сыграть несколько партий подряд без перезапуска")]
    public void SeveralGamesInARow()
    {
        var output = RunApplication(
            """
            \s
            2
            67
            3
            кулёк
            н
            д
            2
            67
            3
            кулёк
            н
            н
            """);

        var winsCount = CountOccurrences(output, "Поздравляю! Вы выиграли!");
        Assert.Equal(2, winsCount);
    }

    [Fact(DisplayName = "Детерминированный режим даёт предсказуемый вывод для автопроверки")]
    public void DeterministicModeProducesPredictableOutput()
    {
        const string input =
            """
            \s
            2
            67
            3
            кулёк
            н
            н
            """;

        var firstRun = RunApplication(input);
        var secondRun = RunApplication(input);

        Assert.Equal(firstRun, secondRun);
        Assert.Contains("Поздравляю! Вы выиграли!", firstRun);
    }

    private static string RunApplication(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        try
        {
            Console.SetIn(reader);
            Console.SetOut(writer);

            WordleApplication.Run();

            return writer.ToString();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }

    private static int CountOccurrences(string text, string value)
    {
        return text.Split(value).Length - 1;
    }
}