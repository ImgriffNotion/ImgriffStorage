using Azure.Storage.Blobs;
using ImgriffStorage.Models.BlobFiles;
using Microsoft.Extensions.Options;

namespace ImgriffStorage.Services.AzureBlobStorageServices
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IOptions<AzureBlobSettings> options)
        {
            var blobServiceClient = new BlobServiceClient(options.Value.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(string userId, IFormFile file)
        {
            string blobName = $"{userId}/{file.FileName}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
        }


        public async Task<string> GetFileUrlAsync(string userId, string filename)
        {
            string blobName = $"{userId}/{filename}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return null;

            Uri sasUri = blobClient.GenerateSasUri(
                Azure.Storage.Sas.BlobSasPermissions.Read,
                DateTimeOffset.UtcNow.AddYears(30));

            return sasUri.ToString();
        }

        public async Task<IEnumerable<BlobFileInfo>> ListUserFilesAsync(string userId)
        {
            var result = new List<BlobFileInfo>();

            await foreach (var blob in _containerClient.GetBlobsAsync(prefix: $"{userId}/"))
            {
                var filename = Path.GetFileName(blob.Name);
                var blobClient = _containerClient.GetBlobClient(blob.Name);
                result.Add(new BlobFileInfo
                {
                    FileName = filename,
                    Url = blobClient.Uri.ToString()
                });
            }

            return result;
        }


        public async Task DeleteFileAsync(string userId, string filename)
        {
            var blobClient = _containerClient.GetBlobClient($"{userId}/{filename}");
            await blobClient.DeleteIfExistsAsync();
        }

    }
}
