using MmsApiV2.TrainerTasks.Requests;

namespace MmsApiV2.Tests.UnitTests.TrainerTasks.Requests;

[Category("Unit")]
[TestOf(typeof(TasksRequest))]
[TestFixture]
public class TasksRequestTests
{
    [Test]
    public void ToQueryParams_ShouldReturnEmptyString_WhenModifiedSinceIsNull()
    {
        // Arrange
        var tasksRequest = new TasksRequest();

        // Act
        var result = tasksRequest.ToQueryParams();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ToQueryParams_ShouldReturnExpectedQueryString_WhenModifiedSinceIsNotNull()
    {
        // Arrange
        var tasksRequest = new TasksRequest { ModifiedSince = "2022-01-01T00:00:00Z" };

        // Act
        var result = tasksRequest.ToQueryParams();

        // Assert
        Assert.That(result, Is.EqualTo("?modifiedSince=2022-01-01T00:00:00Z"));
    }
}
