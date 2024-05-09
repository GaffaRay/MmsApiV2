using MmsApiV2.GymVisits.Models;
using MmsApiV2.MemberAccounts.Models;
using MmsApiV2.MemberAccounts.Requests;
using MmsApiV2.Products.Models;
using MmsApiV2.PushNotifications.Models;
using MmsApiV2.Rfids.Models;
using MmsApiV2.TrainerTasks.Models;
using MmsApiV2.TrainerTasks.Requests;
using MmsApiV2.Webhooks.Models;

namespace MmsApiV2;

/// <summary>
/// Interface for MmsApiV2
/// </summary>
public interface IMmsApiV2Client
{
    #region MemberAccounts
    /// <summary>
    /// Retrieves an account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="MemberAccountDTO"/> object.</returns>
    Task<MemberAccountDTO?> RetrieveAnAccount(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to update.</param>
    /// <param name="memberAccount">The updated member account data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="MemberAccountDTO"/> object.</returns>
    /// <returns></returns>
    Task<MemberAccountDTO?> UpdateAnAccount(string accountId, MemberAccountDTO memberAccount, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAnAccount(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to delete.</param>
    /// <param name="eraseMemberData">If true, then all member data that belongs to a gym (personal data, contract, rfid, product bookings) will be removed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAnAccount(string accountId, bool eraseMemberData, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a a profile picture to the specified user using accountId (Supported formats: JPG, PNG. Max file size: 16MB).
    /// </summary>
    /// <param name="accountId">The ID of the account to which the image will be uploaded.</param>
    /// <param name="fileBytes">The byte array of the image file to be uploaded.</param>
    /// <param name="fileName">The name of the image file to be uploaded.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UploadAccountImage(string accountId, byte[] fileBytes, string fileName, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a a profile picture to the specified user using accountId (Supported formats: JPG, PNG. Max file size: 16MB).
    /// </summary>
    /// <param name="accountId">The ID of the account to which the image will be uploaded.</param>
    /// <param name="multipartFormDataContent">The multipart form data content of the image file to be uploaded.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UploadAccountImage(string accountId, MultipartFormDataContent multipartFormDataContent, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a profile picture of the specified user using accountId.
    /// </summary>
    /// <param name="accountId">The ID of the account from which the image will be deleted.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAccountImage(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Lists all member accounts with a paginated response. Result is always ordered by the updated timestamp (DESC).
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PageableResponseMemberAccountDTO"/> object.</returns>
    Task<PageableResponseMemberAccountDTO?> ListMemberAccounts(MemberAccountsRequest memberAccountsRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new member account.
    /// </summary>
    /// <param name="memberAccount">The member account data to create.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="MemberAccountDTO"/> object.</returns>
    Task<MemberAccountDTO?> CreateAnAccount(MemberAccountDTO memberAccount, CancellationToken cancellationToken);

    /// <summary>
    /// Sets the roles for a specific member account.
    /// </summary>
    /// <param name="accountId">The ID of the account for which the roles will be set.</param>
    /// <param name="roles">The roles to be assigned to the account.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetAccountRoles(string accountId, RoleAssignmentDTO roles, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a member account by its membership ID.
    /// </summary>
    /// <param name="membershipId">The membership ID of the account to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="MemberAccountDTO"/> object of the retrieved account.</returns>
    Task<MemberAccountDTO?> RetrieveAnAccountByMembershipId(string membershipId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a member account by its email.
    /// </summary>
    /// <param name="email">The email of the account to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="MemberAccountDTO"/> object of the retrieved account.</returns>
    Task<MemberAccountDTO?> RetrieveAnAccountByEmail(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Migrates a member account form V1 API.
    /// </summary>
    /// <param name="legacyUserId">The old egym user id for V1.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="MemberAccountDTO"/> object of the migrated account.</returns>
    Task<MemberAccountDTO?> MigrateAnAccount(long legacyUserId, CancellationToken cancellationToken);
    #endregion MemberAccounts

    #region Rfids
    /// <summary>
    /// Retrieves the RFID associated with a specific user.
    /// </summary>
    /// <param name="accountId">The ID of the account for which the RFID will be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of <see cref="RfidDTO"/> objects associated with the user.</returns>
    Task<IEnumerable<RfidDTO>?> RetrieveUserRfids(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates all the RFID associated with a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account for which the RFIDs will be updated.</param>
    /// <param name="rfids">The new RFIDs to be associated with the user.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAllRfidsOfUser(string accountId, IEnumerable<RfidDTO> rfids, CancellationToken cancellationToken);

    /// <summary>
    /// Updates all the RFIDs associated with a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account for which the RFIDs will be updated.</param>
    /// <param name="rfidList">The new list of RFIDs to be associated with the account.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAllRfidsOfUser(string accountId, RfidListDTO rfidList, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new RFID to a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account to which the RFID will be added.</param>
    /// <param name="rfid">The RFID to be added.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="RfidDTO"/> object of the added RFID.</returns>
    Task<RfidDTO?> AddRfid(string accountId, RfidDTO rfid, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an RFID from a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account from which the RFID tag will be removed.</param>
    /// <param name="rfid">The RFID to be removed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveRfid(string accountId, RfidDTO rfid, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an RFID from a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account from which the RFID will be removed.</param>
    /// <param name="rfid">The RFID to be removed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveRfid(string accountId, string rfid, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an RFID with a specific format from a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account from which the RFID tag will be removed.</param>
    /// <param name="rfid">The RFID to be removed.</param>
    /// <param name="tagFormat">The format of the RFID to be removed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveRfid(string accountId, string rfid, string tagFormat, CancellationToken cancellationToken);
    #endregion Rfids

    #region GymVisits
    /// <summary>
    /// Informs Egym system that the member has left the gym.
    /// </summary>
    /// <param name="accountId">The ID of the account for the member checking out.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MemberGymCheckOut(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Informs Egym system that the member has left the gym.
    /// </summary>
    /// <param name="accountId">The ID of the account for the member checking out.</param>
    /// <param name="userPresence">The user presence data for a checkout that happened in the past.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MemberGymCheckOut(string accountId, UserPresenceDTO userPresence, CancellationToken cancellationToken);

    /// <summary>
    /// Informs Egym system that the member has entered the gym.
    /// </summary>
    /// <param name="accountId">The ID of the account for the member checking in.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MemberGymCheckIn(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Informs Egym system that the member has entered the gym.
    /// </summary>
    /// <param name="accountId">The ID of the account for the member checking in.</param>
    /// /// <param name="userPresence">The user presence data for a checkin that happened in the past.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MemberGymCheckIn(string accountId, UserPresenceDTO userPresence, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the admission details for a specific member.<br/>
    /// Only usable for members with membership type “CORPORATE_FITNESS”.<br/>
    /// It verifies if the specified member can have access to the desired gym.<br/>
    /// The MMS should use the response status to allow or block the member from entering the gym.<br/>
    /// As a side-effect this access is logged in the EGYM Corporate Fitness system if the admission was granted.
    /// </summary>
    /// <param name="accountId">The ID of the account for the member.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AdmissionDTO"/> object for the member.</returns>
    Task<AdmissionDTO?> MemberAdmission(string accountId, CancellationToken cancellationToken);
    #endregion GymVisits

    #region ProductsBooking
    /// <summary>
    /// Retrieves the products activated by a specific user in the gym that are active now or in the future.
    /// </summary>
    /// <param name="accountId">The ID of the account.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of <see cref="UserProductDTO"/>s activated by the user.</returns>
    Task<IEnumerable<UserProductDTO>?> RetrieveUserActivatedProducts(string accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Activates or updates a product for a specific account in a gym. The product is active starting from the specified start date to the end date in Timezone of the club.
    /// </summary>
    /// <param name="accountId">The ID of the account for which the product is being activated or updated.</param>
    /// <param name="userProduct">The product to be activated or updated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the activated/updated <see cref="UserProductDTO"/>.</returns>
    Task<UserProductDTO?> ActivateOrUpdateProduct(string accountId, UserProductDTO userProduct, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the products available in the gym that can be activated for a user.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of <see cref="ProductDTO"/>s available in the gym.</returns>
    Task<IEnumerable<ProductDTO>?> RetrieveGymProducts(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the details of a specific product.
    /// </summary>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the details of the <see cref="ProductDTO"/>.</returns>
    Task<ProductDTO?> ProductDetails(string productId, CancellationToken cancellationToken);

    /// <summary>
    /// Deactivates a specific product for a given account.
    /// </summary>
    /// <param name="accountId">The ID of the account for which the product is being deactivated.</param>
    /// <param name="productId">The ID of the product to be deactivated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeactivateProduct(string accountId, string productId, CancellationToken cancellationToken);
    #endregion ProductsBooking

    #region TrainerTasks
    /// <summary>
    /// Retrieves the details of a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the details of the <see cref="TaskDTO"/>.</returns>
    Task<TaskDTO?> RetrieveTaskDetails(string taskId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task to be updated.</param>
    /// <param name="task">The updated task details.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="TaskDTO"/> details.</returns>
    Task<TaskDTO?> UpdateTask(string taskId, TaskDTO task, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all <see cref="TaskDTO"/>s.</returns>
    Task<IEnumerable<TaskDTO>?> RetrieveAllTasks(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tasks based on the provided request parameters.
    /// </summary>
    /// <param name="request">The request parameters for retrieving tasks.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all <see cref="TaskDTO"/>s.</returns>
    Task<IEnumerable<TaskDTO>?> RetrieveAllTasks(TasksRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="task">The details of the task to be created.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the details of the created <see cref="TaskDTO"/>.</returns>
    Task<TaskDTO?> CreateTask(TaskDTO task, CancellationToken cancellationToken);
    #endregion TrainerTasks

    #region Webhooks
    /// <summary>
    /// Retrieves a specific webhook entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the webhook entry.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the details of the <see cref="WebhookResponseDTO"/> if found.</returns>
    Task<WebhookResponseDTO?> RetrieveWebhookEntryById(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a specific webhook entry.
    /// </summary>
    /// <param name="id">The ID of the webhook entry to be updated.</param>
    /// <param name="webhookRequest">The updated webhook entry details.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="WebhookResponseDTO"/> if the operation was successful.</returns>
    Task<WebhookResponseDTO?> UpdateWebhookEntry(long id, WebhookRequestDTO webhookRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a specific webhook entry.
    /// </summary>
    /// <param name="id">The ID of the webhook entry to be deleted.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteWebhook(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all existing webhooks.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all existing <see cref="WebhookResponseDTO"/>s.</returns>
    Task<IEnumerable<WebhookResponseDTO>?> RetrieveExistingWebhooks(CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new webhook URL.
    /// </summary>
    /// <param name="webhookRequest">The details of the webhook to be created.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the details of the created <see cref="WebhookResponseDTO"/>.</returns>
    Task<WebhookResponseDTO?> CreateWebhookUrl(WebhookRequestDTO webhookRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Triggers a specific webhook.
    /// </summary>
    /// <param name="id">The ID of the webhook to be triggered.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task TriggerWebhook(long id, CancellationToken cancellationToken);
    #endregion Webhooks

    #region PushNotifications
    /// <summary>
    /// Sends a notification using a specific account ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to which the notification will be sent.</param>
    /// <param name="partnerPushNotification">The details of the notification to be sent.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendNotificationUsingAccountId(string accountId, PartnerPushNotificationDTO partnerPushNotification, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a notification using a specific membership ID.
    /// </summary>
    /// <param name="membershipId">The membershipId of the account to which the notification will be sent.</param>
    /// <param name="partnerPushNotification">The details of the notification to be sent.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendNotificationUsingMembershipId(string membershipId, PartnerPushNotificationDTO partnerPushNotification, CancellationToken cancellationToken);
    #endregion PushNotifications
}
