using Wordle.App;

namespace Wordle.Tests.Mandatory.Mr1;

/// <summary>Обязательные тесты: словарь.</summary>
public class DictionaryTest
{
    private const string FileName = "dictionary.txt";
    private readonly List<string> _dictionary = GetDictionary.Get(FileName);

    [Fact(DisplayName = "Словарь содержит не меньше 50 слов")]
    public void DictionaryContainsAtLeastFiftyWords()
    {
        Assert.True(_dictionary.Count >= 50);
    }

    [Fact(DisplayName = "Все слова словаря состоят ровно из 5 букв")]
    public void AllWordsAreExactlyFiveLettersLong()
    {
        Assert.All(_dictionary, word => Assert.True(word.Length == 5));
    }

    [Fact(DisplayName = "Пустой словарь приводит к ошибке, а не к запуску игры без слова")]
    public void EmptyDictionaryIsRejected()
    {
        const string emptyFailName = "empty.txt";
        File.WriteAllText(emptyFailName, "");
        try
        {
            Assert.Throws<InvalidOperationException>(() => GetDictionary.Get(emptyFailName));
        }
        finally
        {
            File.Delete(emptyFailName);
        }
    }
}