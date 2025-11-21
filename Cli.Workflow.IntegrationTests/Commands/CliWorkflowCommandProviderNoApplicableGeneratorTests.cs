using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Commands;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cli.Workflow.IntegrationTests.Commands;

[TestFixture]
public class CliWorkflowCommandProviderNoApplicableGeneratorTests
{
    private record TestCliCommand(int Number) : CliCommand;
    
    private class TestCliCommandGeneratorA : ICliCommandGenerator<TestCliCommand>
    {
        public bool CanGenerate(CliInstruction instruction, List<CliCommandProperty> properties)
            => instruction.SubInstructionName == "1";

        public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
            => new TestCliCommand(1);
    }
    
    private class TestCliCommandGeneratorB : ICliCommandGenerator<TestCliCommand>
    {
        public bool CanGenerate(CliInstruction instruction, List<CliCommandProperty> properties)
            => instruction.SubInstructionName == "2";
        
        public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
            => new TestCliCommand(2);
    }
    
    private IServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;
    private CliWorkflowCommandProvider _cliWorkflowCommandProvider;
    
    private TestCliCommand _testCliCommand;
    private TestCliCommandGeneratorA _testCliCommandGeneratorA;
    private TestCliCommandGeneratorB _testCliCommandGeneratorB;
    
    [SetUp]
    public void SetUp()
    {
        _testCliCommand = new TestCliCommand(0);
        _testCliCommandGeneratorA = new TestCliCommandGeneratorA();
        _testCliCommandGeneratorB = new TestCliCommandGeneratorB();
        
        var serviceKey = _testCliCommand.GetInstructionName();
        _serviceCollection = new ServiceCollection();
        _serviceCollection
            .AddKeyedSingleton<IUnidentifiedCliCommandGenerator>(
                serviceKey,
                _testCliCommandGeneratorA)
            .AddKeyedSingleton<IUnidentifiedCliCommandGenerator>(
                serviceKey,
                _testCliCommandGeneratorB);
        
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        
        _cliWorkflowCommandProvider = new CliWorkflowCommandProvider(_serviceProvider);
    }
    
    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
    
    [Test]
    public void GivenCommandAndNoApplicableGenerator_WhenGetCommand_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var instruction = new CliInstruction("/", "test", "non-applicable-sub-command", []);
        var outcomes = new List<CliCommandOutcome>();
        
        // Act & Assert
        Assert.Throws<NoCommandGeneratorException>(() => _cliWorkflowCommandProvider.GetCommand(instruction, outcomes));
    }
}