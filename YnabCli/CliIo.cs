using ConsoleTables;
using YnabCli.Abstractions;

namespace YnabCli;

public class CliIo
{
    public string? Ask()
        => Console.ReadLine();
    
    public void Say(string something)
        => Console.WriteLine(something);
    
    public void Say(CliCommandTableOutcome tableOutcome)
        => Say(tableOutcome.Table.ToString());
    
    public void Say(CliCommandOutputOutcome outputOutcome)
        => Say(outputOutcome.Output);
    
    public void Say(CliCommandNothingOutcome nothingOutcome)
        => Say($"Command resulted in nothing. Last state: {nothingOutcome}");
}