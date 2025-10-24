namespace Cli;

public class CliIo
{
    public string? Ask()
        => Console.ReadLine();
    
    public void Say(string something)
        => Console.WriteLine(something);
}