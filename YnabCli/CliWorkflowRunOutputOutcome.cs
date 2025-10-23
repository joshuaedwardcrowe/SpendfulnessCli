namespace YnabCli;

public class CliWorkflowRunOutputOutcome : CliWorkflowRunOutcome, ICliWorkflowRunOutcome
{
    private readonly string _output;
    
    public CliWorkflowRunOutputOutcome(string output, CliIo cliIo)
        : base(CliWorkflowRunOutcomeKind.Output, cliIo)
    {
        _output = output;
    }

    public void Do() => CliIo.Say(_output);
}