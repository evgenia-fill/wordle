using Wordle.App;

Console.Write("Введите seed, по которому выбирается слово из словаря >> ");
var seed = int.Parse(Console.ReadLine());

Console.Write("Введите максимальное количество попыток >> ");
var maxAttempts = int.Parse(Console.ReadLine());

const string fileName = "dictionary.txt";

var dictionary = WordleDictionary.LoadFromFile(fileName);
var correctWord = WordSelector.Choose(seed, dictionary);
var engine = new GameEngine(dictionary);

try
{
    var session = new GameSession(maxAttempts, correctWord);
    var consoleGame = new ConsoleGame(engine);
    consoleGame.Play(session);
}
catch (ArgumentOutOfRangeException)
{
    Console.WriteLine("Максимальное количество попыток должно быть больше 0!");
}