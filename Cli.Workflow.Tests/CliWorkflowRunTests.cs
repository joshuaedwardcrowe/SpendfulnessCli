using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Abstractions.Validators;
using Cli.Instructions.Parsers;
using Cli.Instructions.Validators;
using Cli.Workflow.Abstractions;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Cli.Workflow.Tests;

[TestFixture]
public class CliWorkflowRunTests
{
    private CliWorkflowRunState _cliWorkflowRunState;
    private Mock<ICliInstructionParser> _cliInstructionParser;
    private Mock<ICliInstructionValidator> _cliInstructionValidator;
    private Mock<ICliWorkflowCommandProvider> _cliWorkflowCommandProvider;
    private Mock<IMediator> _mediator;
    private CliWorkflowRun _classUnderTest;
    
    [SetUp]
    public void SetUp()
    {
        // Arrange
        _cliWorkflowRunState = new CliWorkflowRunState();
        _cliInstructionParser = new Mock<ICliInstructionParser>();
        _cliInstructionValidator = new Mock<ICliInstructionValidator>();
        _cliWorkflowCommandProvider = new Mock<ICliWorkflowCommandProvider>();
        _mediator = new Mock<IMediator>();
        
        _classUnderTest = new CliWorkflowRun(
            _cliWorkflowRunState,
            _cliInstructionParser.Object,
            _cliInstructionValidator.Object,
            _cliWorkflowCommandProvider.Object,
            _mediator.Object
            );
    }
    
    [Test]
    public async Task GivenInvalidAsk_WhenRespondToAsk_ReturnsNothingOutcome()
    {
        // Arrange
        var ask = string.Empty;
        
        // Act
        var outcome = await _classUnderTest.RespondToAsk(ask);
        
        // Assert
        Assert.That(outcome, Is.InstanceOf<CliCommandNothingOutcome>());
    }
    
    [Test]
    public async Task GivenInvalidAsk_WhenRespondToAsk_ChangesStateToInvalidAsk()
    {
        // Arrange
        var ask = string.Empty;
        
        // Act
        _ = await _classUnderTest.RespondToAsk(ask);
        
        // Assert
        var lastStateChange = _cliWorkflowRunState
            .Changes
            .LastOrDefault();
        
        Assert.That(lastStateChange, Is.Not.Null);
        Assert.That(lastStateChange.MovedTo, Is.EqualTo(ClIWorkflowRunStateType.InvalidAsk));
    }
    
    [Test]
    public async Task GivenInstructionParserFails_WhenRespondToAsk_StateChangeBeforeFinishIsInvalidAsk()
    {
        // Arrange
        var ask = "some valid ask";

        _cliInstructionValidator
            .Setup(civ => civ.IsValidInstruction(It.IsAny<CliInstruction>()))
            .Returns(false);
        
        // Act
        _ = await _classUnderTest.RespondToAsk(ask);
        
        // Assert
        var expectedStateChangeTypes = new[]
        {
            ClIWorkflowRunStateType.InvalidAsk,
        };
        
        var stateChangeTypes = _cliWorkflowRunState
            .Changes
            .Select(x => x.MovedTo);

        Assert.That(expectedStateChangeTypes, Is.EqualTo(stateChangeTypes).AsCollection);
    }
    
    [Test]
    public async Task GivenCommandProviderFails_WhenRespondToAsk_StateChangeBeforeFinishIsInvalidAsk()
    {
        // Arrange
        var ask = "some valid ask";
        
        _cliInstructionParser
            .Setup(parser => parser.Parse(It.IsAny<string>()))
            .Returns(new CliInstruction("prefix", "name", null, []));
        
        _cliInstructionValidator
            .Setup(civ => civ.IsValidInstruction(It.IsAny<CliInstruction>()))
            .Returns(true);
        
        _cliWorkflowCommandProvider
            .Setup(provider => provider.GetCommand(It.IsAny<CliInstruction>()))
            .Throws<NoCommandGeneratorException>();
        
        // Act
        _ = await _classUnderTest.RespondToAsk(ask);
        
        // Assert
        var expectedStateChangeTypes = new[]
        {
            ClIWorkflowRunStateType.Running,
            ClIWorkflowRunStateType.InvalidAsk,
            ClIWorkflowRunStateType.Finished
        };
        
        var stateChangeTypes = _cliWorkflowRunState
            .Changes
            .Select(x => x.MovedTo);

        Assert.That(expectedStateChangeTypes, Is.EqualTo(stateChangeTypes).AsCollection);
    }

    [Test]
    public async Task GivenCommandExecutionFails_WhenRespondToAsk_StateChangeBeforeFinishIsExceptional()
    {
        // Arrange
        var ask = "some valid ask";
        
        _cliInstructionParser
            .Setup(parser => parser.Parse(It.IsAny<string>()))
            .Returns(new CliInstruction("prefix", "name", null, []));
        
        _cliInstructionValidator
            .Setup(civ => civ.IsValidInstruction(It.IsAny<CliInstruction>()))
            .Returns(true);
        
        _cliWorkflowCommandProvider
            .Setup(provider => provider.GetCommand(It.IsAny<CliInstruction>()))
            .Returns(new CliCommand());

        _mediator
            .Setup(mediator => mediator.Send(It.IsAny<CliCommand>(), It.IsAny<CancellationToken>()))
            .Throws<Exception>();
        
        // Act
        _ = await _classUnderTest.RespondToAsk(ask);
        
        // Assert
        var expectedStateChangeTypes = new[]
        {
            ClIWorkflowRunStateType.Running,
            ClIWorkflowRunStateType.Exceptional,
            ClIWorkflowRunStateType.Finished
        };
        
        var stateChangeTypes = _cliWorkflowRunState
            .Changes
            .Select(x => x.MovedTo);

        Assert.That(expectedStateChangeTypes, Is.EqualTo(stateChangeTypes).AsCollection);
    }
}

