namespace YnabCli.Instructions.Extensions;

public static class StringExtensions
{
    public static bool AnyLetters(this string argumentValue)
        => argumentValue
            .ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .Any(char.IsLetter);
}