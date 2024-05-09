using MmsApiV2.MemberAccounts.Requests;

namespace MmsApiV2.Tests.UnitTests.MemberAccounts.Requests;

[Category("Unit")]
[TestOf(typeof(MemberAccountsRequest))]
[TestFixture]
public class MemberAccountsRequestTests
{
    [Test]
    public void ToQueryParams_ShouldReturnCorrectString_WhenDefaultValuesAreSet()
    {
        // Arrange
        var request = new MemberAccountsRequest();

        // Act
        var result = request.ToQueryParams();

        // Assert
        Assert.That(result, Is.EqualTo("?currentGymOnly=true&limit=10&offset=0"));
    }

    [Test]
    public void ToQueryParams_ShouldReturnCorrectString_WhenAllValuesAreSet()
    {
        // Arrange
        var request = new MemberAccountsRequest
        {
            CurrentGymOnly = false,
            FromTimestamp = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            Limit = 20,
            Offset = 5,
            ToTimestamp = new DateTime(2022, 12, 31, 0, 0, 0, DateTimeKind.Unspecified)
        };

        // Act
        var result = request.ToQueryParams();

        // Assert
        Assert.That(result, Is.EqualTo("?currentGymOnly=false&fromTimestamp=2022-01-01T00:00:00.0000000&limit=20&offset=5&toTimestamp=2022-12-31T00:00:00.0000000"));
    }
}
