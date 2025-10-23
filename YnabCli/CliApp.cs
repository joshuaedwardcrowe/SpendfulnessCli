namespace YnabCli;

public abstract class CliApp
{
    private string _cliAppName;
    private readonly CliWorkflowEngine _workflowEngine;

    public CliApp(string cliAppName, CliWorkflowEngine workflowEngine)
    {
        _cliAppName = cliAppName;
        _workflowEngine = workflowEngine;
    }
    
    public Task Run()
    {
        var workflow = _workflowEngine.CreateWorkflow(_cliAppName);

        var run = workflow.CreateRun();

        // do
        // {
        //     currentRun.
        // }
        // while (workflow.State == ClIWorkflowState.Running)
        // {
        //     
        // }
        

        // Default state for now.
        return Task.CompletedTask;
    }

    
    protected abstract Task RunCli();
}