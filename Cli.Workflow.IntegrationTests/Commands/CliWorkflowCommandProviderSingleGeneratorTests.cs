using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Commands;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cli.Workflow.Tests.Commands;

[TestFixture]
public class CliWorkflowCommandProviderSingleGeneratorTests
{
    private record TestCliCommand : CliCommand;
    
    private class TestCliCommandGenerator : ICliCommandGenerator<TestCliCommand>
    {
        public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
            => new TestCliCommand();
    }
    
    private IServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;
    private CliWorkflowCommandProvider _cliWorkflowCommandProvider;
    
    private TestCliCommand _cliCommand;
    private TestCliCommandGenerator _cliCommandGenerator;
    
    [SetUp]
    public void SetUp()
    {
        _cliCommand = new TestCliCommand();
        _cliCommandGenerator = new TestCliCommandGenerator();
        
        _serviceCollection = new ServiceCollection();
        _serviceCollection
            .AddKeyedSingleton<IUnidentifiedCliCommandGenerator>(
                _cliCommand.GetInstructionName(),
                _cliCommandGenerator);
        
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        
        _cliWorkflowCommandProvider = new CliWorkflowCommandProvider(_serviceProvider);
    }
    
    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
    
    [Test]
    public void GivenCommandWithSingleGenerator_WhenGetCommand_ThenReturnsExpectedCommandInstance()
    {
        // Arrange
        var instruction = new CliInstruction("/", "test", null, []);
        var outcomes = new List<CliCommandOutcome>();
        
        // Act
        var result = _cliWorkflowCommandProvider.GetCommand(instruction, outcomes);
        
        // Assert
        Assert.That(result, Is.InstanceOf<TestCliCommand>());
    }
}