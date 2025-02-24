using ConsoleTables;

namespace YnabCli.Commands.Reporting.SpareMoney;

public class SpareMoneyHelpCommandHandler(CommandHelpViewModelBuilder commandHelpViewModelBuilder)
    : CommandHandler, ICommandHandler<SpareMoneyHelpCommand>
{
    public Task<ConsoleTable> Handle(SpareMoneyHelpCommand request, CancellationToken cancellationToken)
    {
        var aggregator = new SpareMoneyCommandHelpAggregator();
        
        var viewModel = commandHelpViewModelBuilder
            .AddAggregator(aggregator)
            .AddRowCount(false)
            .Build();
        
        var compilation = Compile(viewModel);

        return Task.FromResult(compilation);
    }
}