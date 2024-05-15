using MmsApiV2.Errors;
using MmsApiV2.GymVisits.Models;
using MmsApiV2.MemberAccounts.Models;
using MmsApiV2.MemberAccounts.Requests;
using MmsApiV2.Products.Models;
using MmsApiV2.PushNotifications.Models;
using MmsApiV2.Rfids.Models;
using MmsApiV2.TrainerTasks.Models;
using MmsApiV2.TrainerTasks.Requests;
using MmsApiV2.Webhooks.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MmsApiV2;

/// <summary>
/// Represents the MmsApiV2 class which implements the IMmsApiV2 and IDisposable interfaces.
/// This class is used to interact with the MMS API version 2.
/// </summary>
public class MmsApiV2Client : IMmsApiV2Client, IDisposable
{
    private const string JsonMediaType = "application/json";

    private const string AccountApiRoot = "api/v2/accounts";
    private const string ProductApiRoot = "api/v2/products";
    private const string TaskApiRoot = "api/v2/tasks";
    private const string WebhookApiRoot = "api/v2/webhooks";
    private const string MigrateApiRoot = "api/v2/migrate";

    /// <summary>
    /// The options for deserializing JSON.
    /// </summary>
    protected JsonSerializerOptions deserializeOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    /// <summary>
    /// The options for serializing JSON.
    /// </summary>
    protected JsonSerializerOptions serializeOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// The HTTP client used for API requests.
    /// </summary>
    protected readonly HttpClient httpClient;

    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="MmsApiV2Client"/> class with the specified API key and base URL.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    /// <param name="apiBaseUrl">The base URL for the API.</param>
    public MmsApiV2Client(string apiKey, string apiBaseUrl = ApiBaseUrls.Prod) : this(new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl),
        DefaultRequestHeaders =
        {
            { "Accept", JsonMediaType },
            { "x-api-key", apiKey}
        }
    })
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MmsApiV2Client"/> class using the provided HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient used for making API requests.</param>
    public MmsApiV2Client(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    #region Helpers
    /// <summary>
    /// Sends a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the content to be deserialized.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the deserialized response body.</returns>
    protected virtual async Task<T?> Get<T>(string requestUri, CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await httpClient
            .GetAsync(requestUri, cancellationToken)
            .ConfigureAwait(false);

        return await ProcessHttpResponse<T>(httpResponseMessage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="obj">The object to be serialized and sent in the body of the request. If null, an empty string is sent.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual async Task Post(string requestUri, object? obj, CancellationToken cancellationToken)
    {
        StringContent? content;

        if (obj is not null)
        {
            var json = JsonSerializer.Serialize(obj, serializeOptions);

            content = new StringContent(json, Encoding.UTF8, JsonMediaType);
        }
        else
        {
            content = new StringContent(string.Empty, Encoding.UTF8, JsonMediaType);
        }

        using var httpResponseMessage = await httpClient
            .PostAsync(requestUri, content, cancellationToken)
            .ConfigureAwait(false);

        await ProcessHttpResponse(httpResponseMessage, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the content to be deserialized.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the deserialized response body.</returns>
    protected virtual async Task<T?> Post<T>(string requestUri, CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await httpClient
            .PostAsync(requestUri, null, cancellationToken)
            .ConfigureAwait(false);

        return await ProcessHttpResponse<T>(httpResponseMessage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a POST request with a JSON payload to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the content to be deserialized.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="obj">The object to be serialized and sent in the body of the request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the deserialized response body.</returns>
    protected virtual async Task<T?> Post<T>(string requestUri, object obj, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(obj, serializeOptions);

        using var httpResponseMessage = await httpClient
            .PostAsync(requestUri, new StringContent(json, Encoding.UTF8, JsonMediaType), cancellationToken)
            .ConfigureAwait(false);

        return await ProcessHttpResponse<T>(httpResponseMessage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PUT request with a JSON payload to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="obj">The object to be serialized and sent in the body of the request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual async Task Put(string requestUri, object obj, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(obj, serializeOptions);

        using var httpResponseMessage = await httpClient
            .PutAsync(requestUri, new StringContent(json, Encoding.UTF8, JsonMediaType), cancellationToken)
            .ConfigureAwait(false);

        await ProcessHttpResponse(httpResponseMessage, cancellationToken);
    }

    /// <summary>
    /// Sends a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the content to be deserialized.</typeparam>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="obj">The object to be serialized and sent in the body of the request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains the deserialized response body.</returns>
    protected virtual async Task<T?> Put<T>(string requestUri, object obj, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(obj, serializeOptions);

        using var httpResponseMessage = await httpClient
            .PutAsync(requestUri, new StringContent(json, Encoding.UTF8, JsonMediaType), cancellationToken)
            .ConfigureAwait(false);

        return await ProcessHttpResponse<T>(httpResponseMessage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual async Task Delete(string requestUri, CancellationToken cancellationToken)
    {
        using var httpResponseMessage = await httpClient
            .DeleteAsync(requestUri, cancellationToken)
            .ConfigureAwait(false);

        await ProcessHttpResponse(httpResponseMessage, cancellationToken);
    }

    /// <summary>
    /// Asynchronously processes the HTTP response message and returns the deserialized content of type T.
    /// </summary>
    /// <typeparam name="T">The type of the content to be deserialized.</typeparam>
    /// <param name="httpResponseMessage">The HTTP response message to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The deserialized content of type T if the HTTP response is successful; otherwise throws an <see cref="MmsApiV2Exception"/>.</returns>
    /// <exception cref="MmsApiV2Exception">Thrown when the HTTP response is not successful.</exception>
    protected virtual async Task<T?> ProcessHttpResponse<T>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        var content = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<T>(content, deserializeOptions);
        }

        var message = httpResponseMessage.ReasonPhrase;

        if (httpResponseMessage.Content.Headers.ContentLength > 0)
        {
            message = Encoding.UTF8.GetString(content);
        }

        try
        {
            var error = JsonSerializer.Deserialize<ErrorDTO>(content, deserializeOptions);

            throw new MmsApiV2Exception((int)httpResponseMessage.StatusCode, error, message);
        }
        catch (Exception e)
        {
            throw new MmsApiV2Exception((int)httpResponseMessage.StatusCode, message, e);
        }
    }

    /// <summary>
    /// Asynchronously processes the HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <exception cref="MmsApiV2Exception">Thrown when the HTTP response is not successful.</exception>
    protected virtual async Task ProcessHttpResponse(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return;
        }

        var content = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);

        var message = httpResponseMessage.ReasonPhrase;

        if (httpResponseMessage.Content.Headers.ContentLength > 0)
        {
            message = Encoding.UTF8.GetString(content);
        }

        try
        {
            var error = JsonSerializer.Deserialize<ErrorDTO>(content, deserializeOptions);

            throw new MmsApiV2Exception((int)httpResponseMessage.StatusCode, error, message);
        }
        catch (Exception e)
        {
            throw new MmsApiV2Exception((int)httpResponseMessage.StatusCode, message, e);
        }
    }

    #endregion Helpers

    #region MemberAccounts
    /// <inheritdoc />
    public async Task<MemberAccountDTO?> RetrieveAnAccount(string accountId, CancellationToken cancellationToken) => await Get<MemberAccountDTO>($"{AccountApiRoot}/{accountId}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<MemberAccountDTO?> UpdateAnAccount(string accountId, MemberAccountDTO memberAccount, CancellationToken cancellationToken) => await Put<MemberAccountDTO>($"{AccountApiRoot}/{accountId}", memberAccount, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task DeleteAnAccount(string accountId, CancellationToken cancellationToken) => await DeleteAnAccount(accountId, false, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task DeleteAnAccount(string accountId, bool eraseMemberData, CancellationToken cancellationToken) => await Delete($"{AccountApiRoot}/{accountId}?eraseMemberData={eraseMemberData.ToString().ToLower()}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task UploadAccountImage(string accountId, byte[] fileBytes, string fileName, CancellationToken cancellationToken) => await UploadAccountImage(accountId, new MultipartFormDataContent { { new ByteArrayContent(fileBytes), "file", fileName } }, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task UploadAccountImage(string accountId, MultipartFormDataContent multipartFormDataContent, CancellationToken cancellationToken) => await Put($"{AccountApiRoot}/{accountId}/images", multipartFormDataContent, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task DeleteAccountImage(string accountId, CancellationToken cancellationToken) => await Delete($"{AccountApiRoot}/{accountId}/images", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<PageableResponseMemberAccountDTO?> ListMemberAccounts(MemberAccountsRequest memberAccountsRequest, CancellationToken cancellationToken) => await Get<PageableResponseMemberAccountDTO>(AccountApiRoot + memberAccountsRequest.ToQueryParams(), cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<MemberAccountDTO?> CreateAnAccount(MemberAccountDTO memberAccount, CancellationToken cancellationToken) => await Post<MemberAccountDTO>(AccountApiRoot, memberAccount, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task SetAccountRoles(string accountId, RoleAssignmentDTO roles, CancellationToken cancellationToken) => await Post($"{AccountApiRoot}/{accountId}/roles", roles, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<MemberAccountDTO?> RetrieveAnAccountByMembershipId(string membershipId, CancellationToken cancellationToken) => await Get<MemberAccountDTO>($"{AccountApiRoot}/membership/{membershipId}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<MemberAccountDTO?> RetrieveAnAccountByEmail(string email, CancellationToken cancellationToken) => await Get<MemberAccountDTO>($"{AccountApiRoot}/email/{email}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<MemberAccountDTO?> MigrateAnAccount(long legacyUserId, CancellationToken cancellationToken) => await Post<MemberAccountDTO>($"{MigrateApiRoot}?legacyUserId={legacyUserId}", cancellationToken).ConfigureAwait(false);
    #endregion MemberAccounts

    #region Rfids
    /// <inheritdoc />
    public async Task<IEnumerable<RfidDTO>?> RetrieveUserRfids(string accountId, CancellationToken cancellationToken) => await Get<IEnumerable<RfidDTO>>($"{AccountApiRoot}/{accountId}/rfids", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task UpdateAllRfidsOfUser(string accountId, IEnumerable<RfidDTO> rfids, CancellationToken cancellationToken) => await UpdateAllRfidsOfUser(accountId, new RfidListDTO { Rfids = rfids }, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task UpdateAllRfidsOfUser(string accountId, RfidListDTO rfidList, CancellationToken cancellationToken) => await Put($"{AccountApiRoot}/{accountId}/rfids", rfidList, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<RfidDTO?> AddRfid(string accountId, RfidDTO rfid, CancellationToken cancellationToken) => await Post<RfidDTO>($"{AccountApiRoot}/{accountId}/rfids", rfid, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task RemoveRfid(string accountId, RfidDTO rfid, CancellationToken cancellationToken) => await RemoveRfid(accountId, rfid.Rfid, rfid.TagFormat, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task RemoveRfid(string accountId, string rfid, CancellationToken cancellationToken) => await Delete($"{AccountApiRoot}/{accountId}/rfids/{rfid}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task RemoveRfid(string accountId, string rfid, string tagFormat, CancellationToken cancellationToken) => await Delete($"{AccountApiRoot}/{accountId}/rfids/{rfid}?tagFormat={tagFormat}", cancellationToken).ConfigureAwait(false);
    #endregion Rfids

    #region GymVisits
    /// <inheritdoc />
    public async Task MemberGymCheckOut(string accountId, CancellationToken cancellationToken) => await MemberGymCheckOut(accountId, new UserPresenceDTO { Timestamp = null }, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task MemberGymCheckOut(string accountId, UserPresenceDTO userPresence, CancellationToken cancellationToken) => await Post($"{AccountApiRoot}/{accountId}/checkouts", userPresence, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task MemberGymCheckIn(string accountId, CancellationToken cancellationToken) => await MemberGymCheckIn(accountId, new UserPresenceDTO { Timestamp = null }, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task MemberGymCheckIn(string accountId, UserPresenceDTO userPresence, CancellationToken cancellationToken) => await Post($"{AccountApiRoot}/{accountId}/checkins", userPresence, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<AdmissionDTO?> MemberAdmission(string accountId, CancellationToken cancellationToken) => await Post<AdmissionDTO>($"{AccountApiRoot}/{accountId}/admissions", cancellationToken).ConfigureAwait(false);
    #endregion GymVisits

    #region ProductsBooking
    /// <inheritdoc />
    public async Task<IEnumerable<UserProductDTO>?> RetrieveUserActivatedProducts(string accountId, CancellationToken cancellationToken) => await Get<IEnumerable<UserProductDTO>?>($"{AccountApiRoot}/{accountId}/products", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<UserProductDTO?> ActivateOrUpdateProduct(string accountId, UserProductDTO userProduct, CancellationToken cancellationToken) => await Put<UserProductDTO>($"{AccountApiRoot}/{accountId}/products", userProduct, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<IEnumerable<ProductDTO>?> RetrieveGymProducts(CancellationToken cancellationToken) => await Get<IEnumerable<ProductDTO>?>(ProductApiRoot, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<ProductDTO?> ProductDetails(string productId, CancellationToken cancellationToken) => await Get<ProductDTO>($"{ProductApiRoot}/{productId}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task DeactivateProduct(string accountId, string productId, CancellationToken cancellationToken) => await Delete($"{AccountApiRoot}/{accountId}/products/{productId}", cancellationToken).ConfigureAwait(false);
    #endregion ProductsBooking

    #region TrainerTask
    /// <inheritdoc />
    public async Task<TaskDTO?> RetrieveTaskDetails(string taskId, CancellationToken cancellationToken) => await Get<TaskDTO?>($"{TaskApiRoot}/{taskId}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<TaskDTO?> UpdateTask(string taskId, TaskDTO task, CancellationToken cancellationToken) => await Put<TaskDTO?>($"{TaskApiRoot}/{taskId}", task, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<IEnumerable<TaskDTO>?> RetrieveAllTasks(CancellationToken cancellationToken) => await RetrieveAllTasks(new TasksRequest(), cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<IEnumerable<TaskDTO>?> RetrieveAllTasks(TasksRequest request, CancellationToken cancellationToken) => await Get<IEnumerable<TaskDTO>>(TaskApiRoot + request.ToQueryParams(), cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<TaskDTO?> CreateTask(TaskDTO task, CancellationToken cancellationToken) => await Post<TaskDTO?>(TaskApiRoot, task, cancellationToken).ConfigureAwait(false);
    #endregion TrainerTask

    #region Webhooks
    /// <inheritdoc />
    public async Task<WebhookResponseDTO?> RetrieveWebhookEntryById(long id, CancellationToken cancellationToken) => await Get<WebhookResponseDTO>($"{WebhookApiRoot}/{id}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<WebhookResponseDTO?> UpdateWebhookEntry(long id, WebhookRequestDTO webhookRequest, CancellationToken cancellationToken) => await Put<WebhookResponseDTO>($"{WebhookApiRoot}/{id}", webhookRequest, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task DeleteWebhook(long id, CancellationToken cancellationToken) => await Delete($"{WebhookApiRoot}/{id}", cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<IEnumerable<WebhookResponseDTO>?> RetrieveExistingWebhooks(CancellationToken cancellationToken) => await Get<IEnumerable<WebhookResponseDTO>>(WebhookApiRoot, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<WebhookResponseDTO?> CreateWebhookUrl(WebhookRequestDTO webhookRequest, CancellationToken cancellationToken) => await Post<WebhookResponseDTO>(WebhookApiRoot, webhookRequest, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task TriggerWebhook(long id, CancellationToken cancellationToken) => await Post($"{WebhookApiRoot}/{id}/trigger", null, cancellationToken).ConfigureAwait(false);
    #endregion Webhooks

    #region PushNotifications
    /// <inheritdoc />
    public async Task SendNotificationUsingAccountId(string accountId, PartnerPushNotificationDTO partnerPushNotification, CancellationToken cancellationToken) => await Post($"{AccountApiRoot}/{accountId}/notifications", partnerPushNotification, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task SendNotificationUsingMembershipId(string membershipId, PartnerPushNotificationDTO partnerPushNotification, CancellationToken cancellationToken) => await Post($"{AccountApiRoot}/membership/{membershipId}/notifications", partnerPushNotification, cancellationToken).ConfigureAwait(false);
    #endregion PushNotifications

    #region Dispose
    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="MmsApiV2Client"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="MmsApiV2Client"/> class and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                httpClient?.Dispose();
            }

            isDisposed = true;
        }
    }
    #endregion Dispose
}
