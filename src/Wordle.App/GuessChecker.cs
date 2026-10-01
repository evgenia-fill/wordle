namespace Wordle.App;

public static class GuessChecker
{
    public static IReadOnlyList<LetterStatus> Check(string guess, string correctAnswer)
    {
        var statuses = new[] { LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.NotContained, LetterStatus.NotContained };

        var wordDict = BuildDictionary(correctAnswer);

        MarkCorrectPlace();
        MarkWrongPlace();

        return statuses;


        void MarkCorrectPlace()
        {
            for (var i = 0; i < guess.Length; i++)
            {
                if (guess[i] != correctAnswer[i])
                {
                    continue;
                }

                statuses[i] = LetterStatus.OnCorrectPlace;
                wordDict[correctAnswer[i]] -= 1;
            }
        }

        void MarkWrongPlace()
        {
            for (var i = 0; i < guess.Length; i++)
            {
                if (statuses[i] == LetterStatus.OnCorrectPlace)
                {
                    continue;
                }

                if (!wordDict.TryGetValue(guess[i], out var value) || value == 0)
                {
                    statuses[i] = LetterStatus.NotContained;
                }

                else if (wordDict.ContainsKey(guess[i]))
                {
                    statuses[i] = LetterStatus.OnWrongPlace;
                    wordDict[guess[i]] -= 1;
                }
            }
        }
    }

    private static Dictionary<char, int> BuildDictionary(string correctAnswer)
    {
        var wordDict = new Dictionary<char, int>();
        foreach (var c in correctAnswer)
        {
            wordDict.TryAdd(c, 0);
            wordDict[c] += 1;
        }

        return wordDict;
    }
}