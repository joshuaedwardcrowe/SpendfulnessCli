using YnabCli.Abstractions;

namespace YnabCli;

public abstract class CliApp
{
    // TODO: I can probably set this in the ServiceCollectionExtension based on class name.
    private string _cliAppName;
    private readonly CliWorkflow _cliWorkflow;
    private readonly CliIo _cliIo;

    public CliApp(string cliAppName, CliWorkflow cliWorkflow, CliIo cliIo)
    {
        _cliAppName = cliAppName;
        _cliWorkflow = cliWorkflow;
        _cliIo = cliIo;
    }
    
    public async Task Run()
    {
        await OnRun(_cliWorkflow);
        
        while (_cliWorkflow.State != CliWorkflowState.Stopped)
        {
            var cliWorkflowRun = _cliWorkflow.CreateRun();

            await OnRunCreated(cliWorkflowRun);
            
            var cliWorkflowRunTask =  cliWorkflowRun.Action();
            
            await OnRunStarted(cliWorkflowRun);
            
            SayOutcome(await cliWorkflowRunTask);
        }
    }

    private void SayOutcome(CliCommandOutcome outcome)
    {
        switch (outcome)
        {
            case CliCommandTableOutcome tableOutcome:
                _cliIo.Say(tableOutcome);
                break;
            case CliCommandOutputOutcome outputOutcome:
                _cliIo.Say(outputOutcome);
                break;
            case CliCommandNothingOutcome nothingOutcome:
                _cliIo.Say(nothingOutcome);
                break;
            default:
                throw new UnknownCliCommandOutcomeException(
                    $"{outcome.Kind} outcomes not supported");
        }
    }

    protected virtual Task OnRun(CliWorkflow cliWorkflow)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnRunCreated(CliWorkflowRun cliWorkflowRun)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnRunStarted(CliWorkflowRun cliWorkflowRun)
    {
        return Task.CompletedTask;
    }
}