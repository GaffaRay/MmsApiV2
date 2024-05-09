using Bogus;
using MmsApiV2.Errors;
using MmsApiV2.MemberAccounts.Constants;
using MmsApiV2.MemberAccounts.Models;
using MmsApiV2.MemberAccounts.Requests;
using MmsApiV2.Products.Models;
using MmsApiV2.PushNotifications.Constants;
using MmsApiV2.PushNotifications.Models;
using MmsApiV2.Rfids.Constants;
using MmsApiV2.Rfids.Models;
using MmsApiV2.TrainerTasks.Constants;
using MmsApiV2.TrainerTasks.Models;
using MmsApiV2.Webhooks.Models;

namespace MmsApiV2.Tests.IntegrationTests;

[Category("Integration")]
[TestOf(typeof(MmsApiV2Client))]
[TestFixture]
public class MmsApiV2ClientTests
{
    const string formateDate = "yyyy-MM-dd";
    const string Prefer = "Prefer";
    const string OkStaticallyGenerated = "code=200";
    const string OkDynamicallyGenerated = "code=200, dynamic=true";
    const string CreatedStaticallyGenerated = "code=201";
    const string CreatedDynamicallyGenerated = "code=201, dynamic=true";
    const string NoContentStaticallyGenerated = "code=204";
    const string BadRequestDynamicallyGenerated = "code=400, dynamic=true";
    const string ForbiddenDynamicallyGenerated = "code=403, dynamic=true";
    const string ConflictDynamicallyGenerated = "code=409, dynamic=true";
    const string InternalServerErrorDynamicallyGenerated = "code=500, dynamic=true";
    const long longMinValue = -9007199254740991;
    const long longMaxValue = 9007199254740991;

    private Faker<ContactDTO> _contactFaker;
    private Faker<MembershipDTO> _membershipFaker;
    private Faker<MemberAccountDTO> _memberAccountFaker;
    private Faker<RfidDTO> _rfidFaker;
    private Faker<TaskDTO> _taskFaker;
    private Faker<PartnerPushNotificationDTO> _partnerPushNotificationFaker;
    private Faker _faker;
    private string _accountId;
    private string _apiKey;
    private HttpClient _httpClient;
    private MmsApiV2Client _mmsApiV2;

    [SetUp]
    public void Setup()
    {
        _contactFaker = new Faker<ContactDTO>()
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Street, f => f.Address.StreetName())
            .RuleFor(c => c.StreetNumber, f => f.Address.BuildingNumber())
            .RuleFor(c => c.ZipCode, f => f.Address.ZipCode())
            .RuleFor(c => c.City, f => f.Address.City())
            .RuleFor(c => c.State, f => f.Address.State())
            .RuleFor(c => c.Country, f => f.Address.Country());

        _membershipFaker = new Faker<MembershipDTO>()
            .RuleFor(m => m.MembershipId, f => f.Random.UInt().ToString())
            .RuleFor(m => m.AgreementNumber, f => f.Random.UInt().ToString())
            .RuleFor(m => m.MembershipType, f => f.Random.CollectionItem<string>([MembershipTypes.Basic, MembershipTypes.Premium, MembershipTypes.Prospect]))
            .RuleFor(m => m.MembershipSubType, f => f.Random.AlphaNumeric(10))
            .RuleFor(m => m.EndOfContract, f => f.Date.Future().ToString(formateDate))
            .RuleFor(m => m.StartOfContract, f => f.Date.Past().ToString(formateDate));

        _memberAccountFaker = new Faker<MemberAccountDTO>()
            .RuleFor(m => m.Email, f => f.Person.Email)
            .RuleFor(m => m.FirstName, f => f.Person.FirstName)
            .RuleFor(m => m.LastName, f => f.Person.LastName)
            .RuleFor(m => m.DateOfBirth, f => f.Person.DateOfBirth.ToString(formateDate))
            .RuleFor(m => m.Gender, f => f.Random.CollectionItem<string>([Genders.Male, Genders.Female, Genders.NonBinary]))
            .RuleFor(m => m.Contact, _ => _contactFaker.Generate())
            .RuleFor(m => m.Membership, _ => _membershipFaker.Generate());

        _rfidFaker = new Faker<RfidDTO>()
            .RuleFor(r => r.Rfid, f => "0x" + f.Random.ReplaceNumbers("########"))
            .RuleFor(r => r.TagFormat, f => f.Random.CollectionItem<string>([TagFormats.Hitag1, TagFormats.Mifare, TagFormats.Legic]));

        _taskFaker = new Faker<TaskDTO>()
            .RuleFor(t => t.Name, f => f.Random.Words())
            .RuleFor(t => t.Type, f => f.Random.CollectionItem<string>([TaskTypes.General, TaskTypes.Anamnesis]))
            .RuleFor(t => t.DueDate, f => f.Date.Future().ToString(formateDate))
            .RuleFor(t => t.AuthorId, f => f.Random.Int().ToString())
            .RuleFor(t => t.TargetId, f => f.Random.Int().ToString());

        _partnerPushNotificationFaker = new Faker<PartnerPushNotificationDTO>()
            .RuleFor(p => p.Text, f => f.Random.Words())
            .RuleFor(p => p.Deeplink, f => f.Random.CollectionItem<string>([Deeplinks.Workouts]));

        _faker = new Faker();

        _accountId = _faker.Random.ReplaceNumbers("###-###-###");
        _apiKey = _faker.Random.AlphaNumeric(10);

        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(ApiBaseUrls.MockServer),
            DefaultRequestHeaders =
            {
                { "Accept", "application/json" },
                { "x-api-key", _apiKey }
            }
        };

        _mmsApiV2 = new MmsApiV2Client(_httpClient);
    }

    [Test]
    public async Task RetrieveAnAccount_ShouldReturnMemberAccount_WhenEgymApiReturns200OK()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkStaticallyGenerated);

        // Act
        var memberAccount = await _mmsApiV2.RetrieveAnAccount(_accountId, CancellationToken.None);

        // Assert
        Assert.That(memberAccount, Is.Not.Null);
    }

    [Test]
    public void RetrieveAnAccount_ShouldThrowMmsApiV2Exception_WhenEgymApiReturns400BadRequest()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, BadRequestDynamicallyGenerated);

        // Act
        Task RetrieveAnAccount() => _mmsApiV2.RetrieveAnAccount(_accountId, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<MmsApiV2Exception>(RetrieveAnAccount);
    }

    [Test]
    public async Task UpdateAnAccount_ShouldReturnMemberAccount_WhenEgymApiReturns200OK()
    {
        // Arrange
        var memberAccount = _memberAccountFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkStaticallyGenerated);

        // Act
        var updatedMemberAccount = await _mmsApiV2.UpdateAnAccount(_accountId, memberAccount, CancellationToken.None);

        // Assert
        Assert.That(updatedMemberAccount, Is.Not.Null);
    }

    [Test]
    public void UpdateAnAccount_ShouldThrowMmsApiV2Exception_WhenEgymApiReturns500InternalServerError()
    {
        // Arrange
        var memberAccount = new MemberAccountDTO();

        _httpClient.DefaultRequestHeaders.Add(Prefer, InternalServerErrorDynamicallyGenerated);

        // Act
        Task UpdateAnAccount() => _mmsApiV2.UpdateAnAccount(_accountId, memberAccount, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<MmsApiV2Exception>(UpdateAnAccount);
    }

    [Test]
    public void DeleteAnAccount_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task DeleteAnAccount() => _mmsApiV2.DeleteAnAccount(_accountId, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(DeleteAnAccount);
    }

    [Test]
    public void DeleteAnAccount_ShouldThrowMmsApiV2Exception_WhenEgymApiReturns403Forbidden()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, ForbiddenDynamicallyGenerated);

        // Act
        Task UpdateAnAccount() => _mmsApiV2.DeleteAnAccount(_accountId, true, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<MmsApiV2Exception>(UpdateAnAccount);
    }

    [Test]
    public async Task ListMemberAccounts_ShouldReturnMemberAccounts_WhenEgymApiReturns200OK()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkStaticallyGenerated);

        // Act
        var memberAccountsResponse = await _mmsApiV2.ListMemberAccounts(new MemberAccountsRequest(), CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(memberAccountsResponse, Is.Not.Null);
    }

    [Test]
    public void ListMemberAccounts_ShouldThrowMmsApiV2Exception_WhenEgymApiReturns500InternalServerError()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, ForbiddenDynamicallyGenerated);

        // Act
        Task ListMemberAccounts() => _mmsApiV2.ListMemberAccounts(new MemberAccountsRequest(), CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<MmsApiV2Exception>(ListMemberAccounts);
    }

    [Test]
    public async Task CreateAnAccount_ShouldReturnMemberAccount_WhenEgymApiReturns201Created()
    {
        // Arrange
        var memberAccount = _memberAccountFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, CreatedStaticallyGenerated);

        // Act
        var createdMemberAccount = await _mmsApiV2.CreateAnAccount(memberAccount, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(createdMemberAccount, Is.Not.Null);
    }

    [Test]
    public void CreateAnAccount_ShouldThrowMmsApiV2Exception_WhenEgymApiReturns409Conflict()
    {
        // Arrange
        var memberAccount = _memberAccountFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, ConflictDynamicallyGenerated);

        // Act
        Task CreateAnAccount() => _mmsApiV2.CreateAnAccount(memberAccount, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<MmsApiV2Exception>(CreateAnAccount);
    }

    [Test]
    public void SetAccountRoles_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task SetAccountRoles() => _mmsApiV2.SetAccountRoles(_accountId, new RoleAssignmentDTO(), CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(SetAccountRoles);
    }

    [Test]
    public async Task RetrieveAnAccountByMembershipId_ShouldReturnMemberAccount_WhenEgymApiReturns200OK()
    {
        // Arrange
        var membershipId = _faker.Random.Int().ToString();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkStaticallyGenerated);

        // Act
        var memberAccount = await _mmsApiV2.RetrieveAnAccountByMembershipId(membershipId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(memberAccount, Is.Not.Null);
    }

    [Test]
    public async Task RetrieveAnAccountByEmail_ShouldReturnMemberAccount_WhenEgymApiReturns200OK()
    {
        // Arrange
        var email = _faker.Person.Email;

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkStaticallyGenerated);

        // Act
        var memberAccount = await _mmsApiV2.RetrieveAnAccountByEmail(email, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(memberAccount, Is.Not.Null);
    }

    [Test]
    public async Task MigrateAnAccount_ShouldReturnMemberAccount_WhenEgymApiReturns200OK()
    {
        // Arrange
        var legacyId = _faker.Random.Int();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkStaticallyGenerated);

        // Acts
        var memberAccount = await _mmsApiV2.MigrateAnAccount(legacyId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(memberAccount, Is.Not.Null);
    }

    [Test]
    public async Task RetrieveUserRfids_ShouldReturnRfids_WhenEgymApiReturns200OK()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var rfids = await _mmsApiV2.RetrieveUserRfids(_accountId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(rfids, Is.Not.Null);
    }

    [Test]
    public void UpdateAllRfidsOfUser_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        var rfids = _rfidFaker.GenerateBetween(1, 5);

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        Task UpdateAllRfidsOfUser() => _mmsApiV2.UpdateAllRfidsOfUser(_accountId, rfids, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(UpdateAllRfidsOfUser);
    }

    [Test]
    public async Task AddRfid_ShouldReturnRfid_WhenEgymApiReturns200OK()
    {
        // Arrange
        var rfid = _rfidFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var updatedRfid = await _mmsApiV2.AddRfid(_accountId, rfid, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(updatedRfid, Is.Not.Null);
    }

    [Test]
    public void RemoveRfid_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        var rfid = _rfidFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task RemoveRfid() => _mmsApiV2.RemoveRfid(_accountId, rfid.Rfid, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(RemoveRfid);
    }

    [Test]
    public void MemberGymCheckOut_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task MemberGymCheckOut() => _mmsApiV2.MemberGymCheckOut(_accountId, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(MemberGymCheckOut);
    }

    [Test]
    public void MemberGymCheckIn_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task MemberGymCheckIn() => _mmsApiV2.MemberGymCheckIn(_accountId, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(MemberGymCheckIn);
    }

    [Test]
    public async Task MemberAdmission_ShouldReturnAdmission_WhenEgymApiReturns200Ok()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var admission = await _mmsApiV2.MemberAdmission(_accountId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(admission, Is.Not.Null);
    }

    [Test]
    public async Task RetrieveUserActivatedProducts_ShouldReturnUserProducts_WhenEgymApiReturns200Ok()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var userProducts = await _mmsApiV2.RetrieveUserActivatedProducts(_accountId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(userProducts, Is.Not.Null);
    }

    [Test]
    public async Task ActivateOrUpdateProduct_ShouldReturnProduct_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var product = new UserProductDTO();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var updatedProduct = await _mmsApiV2.ActivateOrUpdateProduct(_accountId, product, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(updatedProduct, Is.Not.Null);
    }

    [Test]
    public async Task RetrieveGymProducts_ShouldReturnGymProducts_WhenEgymApiReturns200Ok()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var gymProducts = await _mmsApiV2.RetrieveGymProducts(CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(gymProducts, Is.Not.Null);
    }

    [Test]
    public async Task ProductDetails_ShouldReturnProduct_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var productId = _faker.Random.Int().ToString();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var product = await _mmsApiV2.ProductDetails(productId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(product, Is.Not.Null);
    }

    [Test]
    public void DeactivateProduct_ShouldReturnNoContent_WhenEgymPiReturns204NoContent()
    {
        // Arrange
        var productId = _faker.Random.Int().ToString();

        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task DeactivateProduct() => _mmsApiV2.DeactivateProduct(_accountId, productId, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(DeactivateProduct);
    }

    [Test]
    public async Task RetrieveTaskDetails_ShouldReturnTask_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var taskId = _faker.Random.Long(longMinValue, longMaxValue).ToString();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var task = await _mmsApiV2.RetrieveTaskDetails(taskId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(task, Is.Not.Null);
    }

    [Test]
    public async Task UpdateTask_ShouldReturnTask_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var taskId = _faker.Random.Long(longMinValue, longMaxValue).ToString();
        var task = _taskFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var updatedTask = await _mmsApiV2.UpdateTask(taskId, task, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(updatedTask, Is.Not.Null);
    }

    [Test]
    public async Task RetrieveAllTasks_ShouldReturnTasks_WhenEgymApiReturns200Ok()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var tasks = await _mmsApiV2.RetrieveAllTasks(CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(tasks, Is.Not.Null);
    }

    [Test]
    public async Task CreateTask_ShouldReturnTask_WhenEgymApiReturns201Created()
    {
        // Arrange
        var task = _taskFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, CreatedDynamicallyGenerated);

        // Act
        var createdTask = await _mmsApiV2.CreateTask(task, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(createdTask, Is.Not.Null);
    }

    [Test]
    public async Task RetrieveWebhookEntryById_ShouldReturnWebhook_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var webhookId = _faker.Random.Long(longMinValue, longMaxValue);

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var webhook = await _mmsApiV2.RetrieveWebhookEntryById(webhookId, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(webhook, Is.Not.Null);
    }

    [Test]
    public async Task UpdateWebhookEntry_ShouldReturnWebhook_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var webhookId = _faker.Random.Long(longMinValue, longMaxValue);
        var webhook = new WebhookRequestDTO()
        {
            Secret = _faker.Random.AlphaNumeric(10),
            Url = _faker.Internet.Url()
        };

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var updatedWebhook = await _mmsApiV2.UpdateWebhookEntry(webhookId, webhook, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(updatedWebhook, Is.Not.Null);
    }

    [Test]
    public void DeleteWebhook_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        var webhookId = _faker.Random.Long(longMinValue, longMaxValue);

        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task DeleteWebhook() => _mmsApiV2.DeleteWebhook(webhookId, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(DeleteWebhook);
    }

    [Test]
    public async Task RetrieveExistingWebhooks_ShouldReturnWebhooks_WhenEgymApiReturns200Ok()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var webhooks = await _mmsApiV2.RetrieveExistingWebhooks(CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(webhooks, Is.Not.Null);
    }

    [Test]
    public async Task CreateWebhookUrl_ShouldReturnWebhook_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var webhook = new WebhookRequestDTO()
        {
            Secret = _faker.Random.AlphaNumeric(10),
            Url = _faker.Internet.Url()
        };

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        var createdWebhook = await _mmsApiV2.CreateWebhookUrl(webhook, CancellationToken.None).ConfigureAwait(false);

        // Assert
        Assert.That(createdWebhook, Is.Not.Null);
    }

    [Test]
    public void TriggerWebhook_ShouldReturnNoContent_WhenEgymApiReturns200Ok()
    {
        // Arrange
        var webhookId = _faker.Random.Long(longMinValue, longMaxValue);

        _httpClient.DefaultRequestHeaders.Add(Prefer, OkDynamicallyGenerated);

        // Act
        Task TriggerWebhook() => _mmsApiV2.TriggerWebhook(webhookId, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(TriggerWebhook);
    }

    [Ignore("This test is ignored because the Egym API does not return a 204 No Content response for this endpoint only a 400 BadRequest.")]
    [Test]
    public void SendNotificationUsingAccountId_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        var notification = _partnerPushNotificationFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task SentNotification() => _mmsApiV2.SendNotificationUsingAccountId(_accountId, notification, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(SentNotification);
    }

    [Test]
    public void SendNotificationUsingMembershipId_ShouldReturnNoContent_WhenEgymApiReturns204NoContent()
    {
        // Arrange
        var membershipId = _faker.Random.Int().ToString();
        var notification = _partnerPushNotificationFaker.Generate();

        _httpClient.DefaultRequestHeaders.Add(Prefer, NoContentStaticallyGenerated);

        // Act
        Task SentNotification() => _mmsApiV2.SendNotificationUsingMembershipId(membershipId, notification, CancellationToken.None);

        // Assert
        Assert.DoesNotThrowAsync(SentNotification);
    }

    [TearDown]
    public void TearDown()
    {
        _mmsApiV2.Dispose();
        _httpClient.Dispose();
    }
}
