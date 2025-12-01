using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Commands;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cli.Workflow.IntegrationTests.Commands;

[TestFixture]
public class CliWorkflowCommandProviderCommandArtefactTests
{
    private record TestCliCommand : CliCommand;

    private class TestCliCommandOutcome() : CliCommandOutcome(CliCommandOutcomeKind.Reusable);
    
    private class TestCliCommandArtefact() : CliCommandArtefact("Test");
    
    private class TestCliCommandArtefactFactory : ICliCommandArtefactFactory
    {
        public bool For(CliCommandOutcome outcome) => outcome is TestCliCommandOutcome;

        public CliCommandArtefact Create(CliCommandOutcome outcome) => new TestCliCommandArtefact();
    }

    private class TestCliCommandFactory : ICliCommandFactory<TestCliCommand>
    {
        public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
            => artefacts.FirstOrDefault(x => x is TestCliCommandArtefact) != null;

        public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
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
            .AddSingleton<ICliCommandArtefactFactory, TestCliCommandArtefactFactory>();
        
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