using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow;

// TODO: Write unit tests.
/// <summary>
/// State machine of a command line interface.
/// </summary>
public class CliWorkflow
{
    public List<CliWorkflowRun> Runs = [];
    
    private readonly IServiceProvider _serviceProvider;
    
    public CliWorkflowStatus Status = CliWorkflowStatus.Started;

    public CliWorkflow(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Create a new run, a sub-state machine of an individual execution.
    /// </summary>
    /// <returns>A sub-state mchine of an individual execution.</returns>
    // TODO: NextRun? Not CreateRun?
    public CliWorkflowRun NextRun()
    {
        var runNeedsToContinue = Runs.LastOrDefault(run => run.State.Is(ClIWorkflowRunStateType.CanContinue));
        
        if (runNeedsToContinue != null)
        {
            return runNeedsToContinue;
        }
        
        // TODO: CLI - Store this somewhere?
        var newlyCreatedRun = CreateNewRun();
        
        Runs.Add(newlyCreatedRun);

        return newlyCreatedRun;
    }

    /// <summary>
    /// Close the state machine.
    /// </summary>
    public void Stop()
    {
        Status = CliWorkflowStatus.Stopped;
    }
    
    private CliWorkflowRun CreateNewRun()
    {
        var state = new CliWorkflowRunState();
        var instructions = new List<CliInstruction>();
        var properties = new Dictionary<string, CliCommandProperty>();
        
        var instructionParser = _serviceProvider.GetRequiredService<CliInstructionParser>();
        var commandProvider = _serviceProvider.GetRequiredService<CliWorkflowCommandProvider>();
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        
        return new CliWorkflowRun(state, instructions, properties, instructionParser, commandProvider, mediator);
    }
} 