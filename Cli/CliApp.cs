using YnabCli;

namespace Cli;

public abstract class CliApp
{
    private readonly CliWorkflow _cliWorkflow;
    private readonly CliCommandOutcomeIo _cliCommandOutcomeIo;

    protected CliApp(CliWorkflow cliWorkflow, CliCommandOutcomeIo cliCommandOutcomeIo)
    {
        _cliWorkflow = cliWorkflow;
        _cliCommandOutcomeIo = cliCommandOutcomeIo;
    }
    
    public async Task Run()
    {
        await OnRun(_cliWorkflow, _cliCommandOutcomeIo);
        
        while (_cliWorkflow.State != CliWorkflowState.Stopped)
        {
            var cliWorkflowRun = _cliWorkflow.CreateRun();

            await OnRunCreated(cliWorkflowRun, _cliCommandOutcomeIo);
            
            var ask = _cliCommandOutcomeIo.Ask();
            
            var cliWorkflowRunTask =  cliWorkflowRun.RespondToAsk(ask);
            
            await OnRunStarted(cliWorkflowRun, _cliCommandOutcomeIo);
            
            _cliCommandOutcomeIo.Say(await cliWorkflowRunTask);
        }
    }

    protected virtual Task OnRun(CliWorkflow cliWorkflow, CliIo cliIo)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnRunCreated(CliWorkflowRun cliWorkflowRun, CliIo cliIo)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnRunStarted(CliWorkflowRun cliWorkflowRun, CliIo cliIo)
    {
        return Task.CompletedTask;
    }
}