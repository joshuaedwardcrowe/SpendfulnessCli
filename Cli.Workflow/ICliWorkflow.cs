using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public interface ICliWorkflow
{
    CliWorkflowStatus Status { get; set; }
    
    List<CliWorkflowRun> Runs { get; set; }
    
    CliWorkflowRun CreateRun();

    void Stop();
}