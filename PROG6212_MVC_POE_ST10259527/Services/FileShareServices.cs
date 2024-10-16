using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System.IO;
using System.Threading.Tasks;

namespace PROG6212_MVC_POE_ST10259527.Services
{
    //-----------------------------------------------------------------------------------------------------
    // FileService class
    //-----------------------------------------------------------------------------------------------------
    public class FileService
    {
        private readonly ShareClient _shareClient;

        public FileService(string connectionString)
        {
            // Create a ShareClient to interact with Azure File Storage
            _shareClient = new ShareClient(connectionString, "lecturerclaimsfiles");
            _shareClient.CreateIfNotExists();
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Upload a file to the file share
        //-----------------------------------------------------------------------------------------------------
        public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
        {
            // Create a reference to a directory in the file share
            var directoryClient = _shareClient.GetDirectoryClient("claims");

            // Ensure the directory exists
            await directoryClient.CreateIfNotExistsAsync();

            // Get a reference to the file in the directory
            var fileClient = directoryClient.GetFileClient(fileName);

            // Upload the file
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(
                new HttpRange(0, fileStream.Length),
                fileStream
            );

            // Return the URL of the uploaded file
            return fileClient.Uri.ToString();
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
