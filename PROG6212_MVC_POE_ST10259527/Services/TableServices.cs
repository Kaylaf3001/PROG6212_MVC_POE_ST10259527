using Azure.Data.Tables;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Services
{
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

        // Add a customer profile to the CustomerProfile table
        public async Task AddEntityAsync(UserProfileModel profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
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

        public async Task AddClaim(ClaimsModel claim)
        {
            await _tableClaimsClient.AddEntityAsync(claim);
        }

        public async Task<List<ClaimsModel>> GetAllClaims()
        {
            List claims = new List<ClaimsModel>();

            await foreach (var claim in _tableClaimsClient.QueryAsync<ClaimsModel>())
            {
                claims.Add(claim);
            } 
            return claims;
        }
    }
}
