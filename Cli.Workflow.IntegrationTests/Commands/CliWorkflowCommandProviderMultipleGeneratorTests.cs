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
public class CliWorkflowCommandProviderMultipleGeneratorTests
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
    [TestCase("1", 1)]
    [TestCase("2", 2)]
    public void GivenSubCommand_WhenGetCommand_ThenReturnsExpectedCommandInstance(string subCommandName, int expectedNumber)
    {
        // Arrange
        var instruction = new CliInstruction("/", "test", subCommandName, []);
        var outcomes = new List<CliCommandOutcome>();
        
        // Act
        var cliCommand = _cliWorkflowCommandProvider.GetCommand(instruction, outcomes);
        
        // Assert
        var testCliCommand = cliCommand as TestCliCommand;
        
        Assert.That(testCliCommand, Is.Not.Null);
        Assert.That(testCliCommand.Number, Is.EqualTo(expectedNumber));
    }
}