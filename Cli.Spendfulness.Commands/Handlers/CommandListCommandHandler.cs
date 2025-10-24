using Cli.Abstractions;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Spendfulness.Commands.Handlers;

public class CommandListCommandHandler : CommandHandler, ICommandHandler<CommandListCommand>
{
    public Task<CliCommandOutcome> Handle(CommandListCommand request, CancellationToken cancellationToken)
    {
        var viewModel = new CliTable();
        
        // TODO: This command list needs storing somewhere else in the future, or achieving through reflection.
        viewModel.Columns.AddRange(["Command", "Description"]);
        viewModel.Rows.AddRange(["/command-list", "Gets a list of commands"]);
        
        var compilation = Compile(viewModel);
        return Task.FromResult<CliCommandOutcome>(compilation);
    }
}