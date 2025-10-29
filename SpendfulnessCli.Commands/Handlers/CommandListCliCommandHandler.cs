using Cli.Abstractions;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Handlers;

public class CommandListCliCommandHandler : CliCommandHandler, ICliCommandHandler<CommandListCliCommand>
{
    public Task<CliCommandOutcome> Handle(CommandListCliCommand request, CancellationToken cancellationToken)
    {
        var viewModel = new CliTable();
        
        // TODO: This command list needs storing somewhere else in the future, or achieving through reflection.
        viewModel.Columns.AddRange(["Command", "Description"]);
        viewModel.Rows.AddRange(["/command-list", "Gets a list of commands"]);
        
        var compilation = Compile(viewModel);
        return Task.FromResult<CliCommandOutcome>(compilation);
    }
}