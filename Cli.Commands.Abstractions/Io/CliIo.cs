namespace Cli.Commands.Abstractions.Io;

public class CliIo
{
    public string? Ask()
        => Console.ReadLine();

    public void Pause()
        => Console.WriteLine();
    
    public void Say(string something)
        => Console.WriteLine(something);
}