using Cli.Workflow.Abstractions;
using Cli.Workflow.Run;

namespace Cli.Workflow;

public interface ICliWorkflow
{
    CliWorkflowStatus Status { get; set; }
    
    List<CliWorkflowRun> Runs { get; set; }
    
    CliWorkflowRun NextRun();

    void Stop();
}