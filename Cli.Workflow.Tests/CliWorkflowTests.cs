using Cli.Instructions.Abstractions.Validators;
using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Cli.Workflow.Tests;

[TestFixture]
public class CliWorkflowTests
{
    private Mock<IServiceProvider> _serviceProviderMock;
    private CliWorkflow _classUnderTest;

    [SetUp]
    public void SetUp()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _classUnderTest = new CliWorkflow(_serviceProviderMock.Object);
    }

    [Test]
    public void GivenCreated_WhenConstructor_HasStartedStatus()
    {
        Assert.That(_classUnderTest.Status, Is.EqualTo(CliWorkflowStatus.Started));
    }
    
    [Test]
    public void GivenCreated_WhenNextRun_CreatesNewRun()
    {
        // Arrange
        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICliInstructionParser)))
            .Returns(new Mock<ICliInstructionParser>().Object);
        
        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICliInstructionValidator)))
            .Returns(new Mock<ICliInstructionValidator>().Object);
        
        _serviceProviderMock
            .Setup(sp =>  sp.GetService(typeof(ICliWorkflowCommandProvider)))
            .Returns(new Mock<ICliWorkflowCommandProvider>().Object);
        
        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IMediator)))
            .Returns(new Mock<IMediator>().Object);
        
        // Act
        var run = _classUnderTest.NextRun();
        
        // Assert
        Assert.That(run, Is.Not.Null);
        Assert.That(_classUnderTest.Runs, Has.Member(run));
    }
    
    [Test]
    public void GivenPriorRunAchievedReusableOutcome_WhenNextRun_GetsThatRun()
    {
        // Arrange
        var reusableRunState = new CliWorkflowRunState();
        reusableRunState.ChangeTo(ClIWorkflowRunStateStatus.Running);
        reusableRunState.ChangeTo(ClIWorkflowRunStateStatus.ReachedReusableOutcome);
        
        var reusableRun = new CliWorkflowRun(
            reusableRunState,
            new Mock<ICliInstructionParser>().Object,
            new Mock<ICliInstructionValidator>().Object,
            new Mock<ICliWorkflowCommandProvider>().Object,
            new Mock<IMediator>().Object);
        
        _classUnderTest.Runs.Add(reusableRun);
        
        // Act
        var nextRun = _classUnderTest.NextRun();
        
        // Assert
        Assert.That(nextRun, Is.EqualTo(reusableRun));
    }
    
    [Test]
    public void GivenRunning_WhenStop_ThenWorkflowStopsRunning()
    {
        // Act
        _classUnderTest.Stop();
        
        // Assert
        Assert.That(_classUnderTest.Status, Is.EqualTo(CliWorkflowStatus.Stopped));
    }
}