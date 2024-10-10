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
            // This assumes you stored the Email as a property in your UserProfileModel
            // You might need to modify this if you are using a different storage approach
            var filter = $"Email eq '{email}'"; // Azure Table Storage OData filter
            var entities = _tableClient.Query<UserProfileModel>(filter);

            // Use a regular foreach loop
            foreach (var entity in entities)
            {
                return entity; // Return the first matching user
            }

            return null; // Return null if no user found
        }
    }
}
