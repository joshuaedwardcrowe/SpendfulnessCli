using Cli.Commands.Abstractions.Io;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;
using Cli.Workflow.Abstractions;

namespace Cli;

public abstract class OriginalCli
{
    private readonly CliWorkflow _workflow;
    private readonly CliCommandOutcomeIo _io;

    protected OriginalCli(CliWorkflow workflow, CliCommandOutcomeIo io)
    {
        _workflow = workflow;
        _io = io;
    }
    
    public async Task Run()
    { 
        OnRun(_workflow, _io);
        
        while (_workflow.Status != CliWorkflowStatus.Stopped)
        {
            var cliWorkflowRun = _workflow.CreateRun();
            
            OnRunCreated(cliWorkflowRun, _io);
            
            var ask = _io.Ask();
            
            var cliWorkflowRunTask =  cliWorkflowRun.RespondToAsk(ask);
            
            OnRunStarted(cliWorkflowRun, _io);

            var comandOutcome = await cliWorkflowRunTask;
            
            _io.Say(comandOutcome);
            
            OnRunComplete(comandOutcome, cliWorkflowRun, _io);
        }
    }

    protected virtual void OnRun(CliWorkflow workflow, CliIo io)
    {
    }

    protected virtual void OnRunCreated(CliWorkflowRun workflowRun, CliIo io)
    {
    }

    protected virtual void OnRunStarted(CliWorkflowRun workflowRun, CliIo io)
    {
    }

    protected virtual void OnRunComplete(CliCommandOutcome outcome, CliWorkflowRun workflowRun, CliIo io)
    {
    }
}