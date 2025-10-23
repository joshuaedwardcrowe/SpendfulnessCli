using Cli.Instructions.Parsers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace YnabCli;

public class CliWorkflow
{
    private readonly IServiceProvider _serviceProvider;
    private List<CliWorkflowRun> _workflowRuns = new List<CliWorkflowRun>();

    public CliWorkflow(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public CliWorkflowRun CreateRun()
    {
        var io = _serviceProvider.GetRequiredService<CliIo>();
        
        var consoleInstructionParser = _serviceProvider.GetRequiredService<ConsoleInstructionParser>();
        
        var commandProvider = _serviceProvider.GetRequiredService<CliCommandProvider>();
        
        // TODO: I'd like to remove the dependency on MediatR one day.
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        
        var run = new CliWorkflowRun(io, consoleInstructionParser, commandProvider, mediator);
        
        _workflowRuns.Add(run);

        return run;
    }
}