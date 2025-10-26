namespace Cli.Abstractions;

public static class StringExtensions
{
    // TODO: I wonder if there's something like this.
    public static bool IsSimilarTo(this string ctrl, string candidate)
    {
        var exact = ctrl.Equals(candidate);
        var similar = ctrl.Contains(candidate);
        
        return exact || similar;
    }
}