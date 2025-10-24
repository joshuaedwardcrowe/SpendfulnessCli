using Cli.Workflow.Abstractions;

namespace Cli;

// TODO: CLI - Naing here could be more explicit.
public record CliWorkflowRunStateChange(
    ClIWorkflowRunState From,
    ClIWorkflowRunState To);