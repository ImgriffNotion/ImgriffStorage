using ImgriffStorage.Models.BlobFiles;

namespace ImgriffStorage.Services.AzureBlobStorageServices
{
    public interface IBlobService
    {
        Task UploadFileAsync(string userId, IFormFile file);
        Task<IEnumerable<BlobFileInfo>> ListUserFilesAsync(string userId);
        Task<string> GetFileUrlAsync(string userId, string filename);
        Task DeleteFileAsync(string userId, string filename);
    }
}
