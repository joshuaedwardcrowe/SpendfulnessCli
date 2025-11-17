using Cli.Instructions.Abstractions.Validators;
using Cli.Instructions.Parsers;
using Cli.Instructions.Validators;
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
    public void CreatedWithStartedStatus()
    {
        Assert.That(_classUnderTest.Status, Is.EqualTo(CliWorkflowStatus.Started));
    }
    
    [Test]
    public void CreatesNewRun()
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
        var run = _classUnderTest.CreateRun();
        
        // Assert
        Assert.That(run, Is.Not.Null);
        Assert.That(_classUnderTest.Runs, Has.Member(run));
    }
}