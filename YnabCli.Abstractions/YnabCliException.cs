namespace YnabCli.Abstractions;

// TODO: Convert to CliException & Cli.Abstractions.
public class YnabCliException : Exception
{
    public YnabCliException(string message) : base(message)
    {
    }
}
