using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Artefacts.Aggregator;
using Cli.Commands.Abstractions.Artefacts.CommandRan;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using NUnit.Framework;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.Commands.Reporting.MonthlySpending;
using SpendfulnessCli.Commands.Reusable.Filter;
using SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;
using SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending.TotalAmount.GreaterThan;
using Ynab;

namespace SpendfulnessCli.Commands.Reusable.Tests.Filter.MonthlySpending.TotalAmount.GreaterThan;

[TestFixture]
public class FilterMonthlySpendingOnTotalAmountGreaterThanCliCommandFactoryTests
{
    private FilterMonthlySpendingOnTotalAmountGreaterThanCliCommandFactory _classUnderTest;

    [SetUp]
    public void SetUp()
    {
        _classUnderTest = new FilterMonthlySpendingOnTotalAmountGreaterThanCliCommandFactory();
    }
    
    [Test]
    public void GivenNoGreaterThanArgument_WhenCanCreateWhen_ThenReturnsExpectedResult()
    {
        // Arrange
        var arguments = new List<CliInstructionArgument>
        {
            new ValuedCliInstructionArgument<string>(
                FilterCliCommand.ArgumentNames.FilterOn,
                FilterMonthlySpendingCliCommand.FilterNames.TotalAmount),
        };
        
        var instruction = new CliInstruction("/", "filter", null, arguments);

        var artefacts = new List<CliCommandArtefact>
        {
            new RanCliCommandArtefact(new MonthlySpendingCliCommand())
        };
        
        // Act
        var result = _classUnderTest.CanCreateWhen(instruction, artefacts);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void GivenNoFilterOnArgument_WhenCanCreateWhen_ThenReturnsExpectedResult()
    {
        // Arrange
        var arguments = new List<CliInstructionArgument>
        {
            new ValuedCliInstructionArgument<decimal>(
                FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand.ArgumentNames.GreaterThan,
                decimal.One)
        };
        
        var instruction = new CliInstruction("/", "filter", null, arguments);

        var artefacts = new List<CliCommandArtefact>
        {
            new RanCliCommandArtefact(new MonthlySpendingCliCommand())
        };
        
        // Act
        var result = _classUnderTest.CanCreateWhen(instruction, artefacts);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void GivenDidNotRanMonthlySpendingCommand_WhenCanCreateWhen_ThenReturnsExpectedResult()
    {
        // Arrange
        var arguments = new List<CliInstructionArgument>
        {
            new ValuedCliInstructionArgument<string>(
                FilterCliCommand.ArgumentNames.FilterOn,
                FilterMonthlySpendingCliCommand.FilterNames.TotalAmount),
            
            new ValuedCliInstructionArgument<decimal>(
                FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand.ArgumentNames.GreaterThan,
                decimal.One)
        };
        
        var instruction = new CliInstruction("/", "filter", null, arguments);

        var artefacts = new List<CliCommandArtefact>();
        
        // Act
        var result = _classUnderTest.CanCreateWhen(instruction, artefacts);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GivenRanMonthlySpendingCommand_WhenCanCreateWhen_ThenReturnsExpectedResult()
    {
        // Arrange
        var arguments = new List<CliInstructionArgument>
        {
            new ValuedCliInstructionArgument<string>(
                FilterCliCommand.ArgumentNames.FilterOn,
                FilterMonthlySpendingCliCommand.FilterNames.TotalAmount),
            
            new ValuedCliInstructionArgument<decimal>(
                FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand.ArgumentNames.GreaterThan,
                decimal.One)
        };
        
        var instruction = new CliInstruction("/", "filter", null, arguments);

        var artefacts = new List<CliCommandArtefact>
        {
            new RanCliCommandArtefact(new MonthlySpendingCliCommand()),
        };
        
        // Act
        var result = _classUnderTest.CanCreateWhen(instruction, artefacts);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void GivenArgumentsAndArtefacts_WhenCreate_ThenReturnsFilterCommand()
    {
        // Arrange
        var arguments = new List<CliInstructionArgument>
        {
            new ValuedCliInstructionArgument<string>(
                FilterCliCommand.ArgumentNames.FilterOn,
                FilterMonthlySpendingCliCommand.FilterNames.TotalAmount),
            
            new ValuedCliInstructionArgument<decimal>(
                FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand.ArgumentNames.GreaterThan,
                decimal.One)
        };
        
        var instruction = new CliInstruction("/", "filter", null, arguments);

        var aggregator = new TransactionMonthTotalListAggregator(new List<Transaction>());
        
        var artefacts = new List<CliCommandArtefact>
        {
            new RanCliCommandArtefact(new MonthlySpendingCliCommand()),
            new ListAggregatorCliCommandArtefact<TransactionMonthTotalAggregate>(aggregator)
        };
        
        // Act
        var result = _classUnderTest.Create(instruction, artefacts);
        
        // Assert
        var filterCommand = result as FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand;
        
        Assert.That(filterCommand, Is.Not.Null);
        Assert.That(filterCommand.FilterOn, Is.EqualTo(FilterMonthlySpendingCliCommand.FilterNames.TotalAmount));
        Assert.That(filterCommand.GreaterThan, Is.EqualTo(decimal.One));
    }
}