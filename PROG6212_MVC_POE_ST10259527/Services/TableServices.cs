using Azure.Data.Tables;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Services
{
    public class TableServices
    {
        private readonly TableClient _tableClient;

        public TableServices(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);

            _tableClient = serviceClient.GetTableClient("userTable");
            _tableClient.CreateIfNotExists();


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
    }

}
