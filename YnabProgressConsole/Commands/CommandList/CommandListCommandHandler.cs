using ConsoleTables;
using YnabProgress.ViewModels;

namespace YnabProgressConsole.Commands.CommandList;

public class CommandListCommandHandler : BaseCommandHandler, ICommandHandler<CommandListCommand>
{
    public Task<ConsoleTable> Handle(CommandListCommand request, CancellationToken cancellationToken)
    {
        var viewModel = new ViewModel();
        
        // TODO: This command list needs storing somewhere else in the future, or achieving through reflection.
        viewModel.Columns.AddRange(["Command", "Description"]);
        viewModel.Rows.AddRange(["/command-list", "Gets a list of commands"]);
        
        var compilation = Compile(viewModel);
        
        return Task.FromResult(compilation);
    }
}