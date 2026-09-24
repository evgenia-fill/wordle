namespace Wordle.App;

public enum GuessResultType
{
    Valid,
    LengthError,
    NotInDictionaryError
}

public class GuessResult
{
    public GuessResultType Type { get; }
    public IReadOnlyList<LetterStatus>? LetterStatuses { get; }

    public GuessResult(GuessResultType type, IReadOnlyList<LetterStatus>? letterStatuses = null)
    {
        Type = type;
        LetterStatuses = letterStatuses;
    }
}