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
        // Get a user by lecturerID
        //-----------------------------------------------------------------------------------------------------
        public async Task<UserProfileModel> GetUserByIDAsync(string lecturerID)
        {
            var filter = $"RowKey eq '{lecturerID}'"; // Ensure this is strongly typed
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
        public async Task UpdateClaimStatus(ClaimsModel claim)
        {
            if (claim != null)
            {
                await _tableClaimsClient.UpdateEntityAsync(claim, ETag.All, TableUpdateMode.Replace);
            }
        }
        //-----------------------------------------------------------------------------------------------------

        public async Task<ClaimsModel> GetClaimById(string claimId)
        {
            try
            {
                var claim = await _tableClaimsClient.GetEntityAsync<ClaimsModel>("Claims", claimId);
                return claim;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }

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
