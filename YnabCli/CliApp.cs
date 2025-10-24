namespace YnabCli;

// TODO: Build CliCommandOutcome... Destructor? Outputter? Manager? Reader?

public abstract class CliApp
{
    // TODO: I can probably set this in the ServiceCollectionExtension based on class name.
    private string _cliAppName;
    private CliWorkflow _cliWorkflow;

    public CliApp(string cliAppName, CliWorkflow cliWorkflow)
    {
        _cliAppName = cliAppName;
        _cliWorkflow = cliWorkflow;
    }
    
    public async Task Run()
    {
        await OnRun(_cliWorkflow);
        
        while (_cliWorkflow.State != CliWorkflowState.Stopped)
        {
            var cliWorkflowRun = _cliWorkflow.CreateRun();

            await OnRunCreated(cliWorkflowRun);
            
            // TODO: Change me to by awaited.
            // This will ask for a command, send it to meditor.
            var cliWorkflowRunTask =  cliWorkflowRun.Action();
            
            await OnRunStarted(cliWorkflowRun);
            
            var cliWorkflowOutcome = await cliWorkflowRunTask;

            // TODO: Have some outcome result thing abstraction.
            // var cliWorkflowRunOutcome = new CliCommandOutputOutcome("Test");
            //
            // // This will log something in the console.
            // cliWorkflowRunOutcome.Do();
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