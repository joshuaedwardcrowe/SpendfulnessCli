using ConsoleTables;

namespace YnabCli;

public class CliWorkflowRunTableOutcome : CliWorkflowRunOutcome, ICliWorkflowRunOutcome
{
    private readonly ConsoleTable _consoleTable;
    
    public CliWorkflowRunTableOutcome(ConsoleTable consoleTable, CliIo cliIo)
        : base(CliWorkflowRunOutcomeKind.Table, cliIo)
    {
        _consoleTable = consoleTable;
    }
    
    public void Do()
    {
        var output = _consoleTable.ToString();
        
        CliIo.Say(output);
    }
}