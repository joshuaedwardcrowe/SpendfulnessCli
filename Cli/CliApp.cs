using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;
using Cli.Workflow.Abstractions;

namespace Cli;

// TODO: Rename to something clever?
// TODO: Write unit tests.
public abstract class CliApp
{
    private readonly CliWorkflow _workflow;
    protected readonly CliCommandOutcomeIo Io;

    protected CliApp(CliWorkflow workflow, CliCommandOutcomeIo io)
    {
        _workflow = workflow;
        Io = io;
    }
    
    public async Task Run()
    { 
        OnSessionStart();
        
        while (_workflow.Status != CliWorkflowStatus.Stopped)
        {
            var run = _workflow.NextRun();
            
            OnRunCreated(run);
            
            var ask = Io.Ask();
            
            var runTask =  run.RespondToAsk(ask);
            
            OnRunStarted(run, ask);

            var outcome = await runTask;
            
            Io.Say(outcome);
            
            OnRunComplete(run, outcome);
        }
        
        OnSessionEnd(_workflow.Runs);
    }

    protected virtual void OnSessionStart()
    {
    }

    protected virtual void OnRunCreated(CliWorkflowRun run)
    {
    }

    protected virtual void OnRunStarted(CliWorkflowRun run, string ask)
    {
    }

    protected virtual void OnRunComplete(CliWorkflowRun run, CliCommandOutcome outcome)
    {
    }
    
    protected virtual void OnSessionEnd(List<CliWorkflowRun> runs)
    {
    }
}