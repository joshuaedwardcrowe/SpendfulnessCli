using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Commands;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cli.Workflow.IntegrationTests.Commands;

[TestFixture]
public class CliWorkflowCommandProviderCommandPropertyTests
{
    private record TestCliCommand : CliCommand;

    private class TestCliCommandOutcome() : CliCommandOutcome(CliCommandOutcomeKind.Reusable);
    
    private class TestCliCommandProperty : CliCommandProperty;
    
    private class TestCliCommandPropertyFactory : ICliCommandPropertyFactory
    {
        public bool CanCreatePropertyWhen(CliCommandOutcome outcome)
            => outcome is TestCliCommandOutcome;

        public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
            => new TestCliCommandProperty();
    }

    private class TestCliCommandFactory : ICliCommandFactory<TestCliCommand>
    {
        public bool CanGenerateWhen(CliInstruction instruction, List<CliCommandProperty> properties)
            => properties.OfType<TestCliCommandProperty>().Any();

        public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
            => new TestCliCommand();
    }
    
    private IServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;
    private CliWorkflowCommandProvider _cliWorkflowCommandProvider;
    
    
    [SetUp]
    public void SetUp()
    {
        var serviceKey = new TestCliCommand().GetInstructionName();
        _serviceCollection = new ServiceCollection();
        _serviceCollection
            .AddKeyedSingleton<IUnidentifiedCliCommandFactory, TestCliCommandFactory>(serviceKey)
            .AddSingleton<ICliCommandPropertyFactory, TestCliCommandPropertyFactory>();
        
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        
        _cliWorkflowCommandProvider = new CliWorkflowCommandProvider(_serviceProvider);
    }
    
    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
    
    [Test]
    public void GivenCommandAndConvertableOutcome_WhenGetCommand_ThenReturnsExpectedCommandInstance()
    {
        // Arrange
        var instruction = new CliInstruction("/", "test", null, []);
        var outcomes = new List<CliCommandOutcome>
        {
            new TestCliCommandOutcome()
        };
        
        // Act
        var result = _cliWorkflowCommandProvider.GetCommand(instruction, outcomes);
        
        // Assert
        Assert.That(result, Is.InstanceOf<TestCliCommand>());
    }
}