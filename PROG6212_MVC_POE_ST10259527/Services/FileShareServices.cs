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
        private readonly ShareServiceClient _shareServiceClient;

        public FileService(string connectionString)
        {
            _shareServiceClient = new ShareServiceClient(connectionString);
        }

        //--------------------------------------------------------------------------------------
        //Uploads Files to file share in azure
        //--------------------------------------------------------------------------------------
        public async Task<string> UploadFileAsync(string shareName, string fileName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            var directoryClient = shareClient.GetRootDirectoryClient();
            await shareClient.CreateIfNotExistsAsync();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);

            return fileClient.Uri.ToString();
        }
        //--------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------
        //
        public async Task<List<string>> ListFilesAsync(string shareName)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync();
            var directoryClient = shareClient.GetRootDirectoryClient();
            var files = new List<string>();
            await foreach (var item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                files.Add(item.Name);
            }
            return files;
        }
        public async Task<Stream> DownloadFileAsync(string shareName, string fileName)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            
            var downloadResponse = await fileClient.DownloadAsync();
            return downloadResponse.Value.Content;
        }

        public async Task DeleteFileAsync(string shareName, string fileName)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            await fileClient.DeleteIfExistsAsync();
        }
    }

}
//-----------------------------------------------End-Of-File----------------------------------------------------
