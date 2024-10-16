using Azure;
using Azure.Data.Tables;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Services
{
    //-----------------------------------------------------------------------------------------------------
    // TableServices class
    //-----------------------------------------------------------------------------------------------------
    public class TableServices
    {
        private readonly TableClient _tableClient;
        private readonly TableClient _tableClaimsClient;

        public TableServices(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);

            _tableClient = serviceClient.GetTableClient("userTable");
            _tableClient.CreateIfNotExists();

            _tableClaimsClient = serviceClient.GetTableClient("lecturersClaims");
            _tableClaimsClient.CreateIfNotExists();

        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Add a customer profile to the CustomerProfile table
        //-----------------------------------------------------------------------------------------------------
        public async Task AddEntityAsync(UserProfileModel profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Get a user by email
        //-----------------------------------------------------------------------------------------------------
        public async Task<UserProfileModel> GetUserByEmailAsync(string email)
        {
            var filter = $"Email eq '{email}'"; // Ensure this is strongly typed
            var entities = _tableClient.Query<UserProfileModel>(filter);

            foreach (var entity in entities)
            {
                return entity;
            }
            return null;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Add a claim to the Claims table
        //-----------------------------------------------------------------------------------------------------
        public async Task AddClaim(ClaimsModel claim)
        {
            await _tableClaimsClient.AddEntityAsync(claim);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Get all claims
        //-----------------------------------------------------------------------------------------------------
        public async Task<List<ClaimsModel>> GetAllClaims()
        {
            List<ClaimsModel> claims = new List<ClaimsModel>();

            await foreach (var claim in _tableClaimsClient.QueryAsync<ClaimsModel>())
            {
                claims.Add(claim);
            } 
            return claims;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Update the status of a claim
        //-----------------------------------------------------------------------------------------------------
        public async Task UpdateClaimStatus(string rowKey, string status)
        {
            // Retrieve the claim by RowKey
            var claim = _tableClaimsClient.Query<ClaimsModel>(c => c.RowKey == rowKey).FirstOrDefault();

            if (claim != null)
            {
                claim.Status = status; // Update the status
                await _tableClaimsClient.UpdateEntityAsync(claim, ETag.All, TableUpdateMode.Replace);
            }
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Get all Users
        //-----------------------------------------------------------------------------------------------------
        public async Task<List<UserProfileModel>> GetAllUsers()
        {
            List<UserProfileModel> users = new List<UserProfileModel>();
            await foreach (var user in _tableClient.QueryAsync<UserProfileModel>())
            {
                users.Add(user);
            }
            return users;
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
