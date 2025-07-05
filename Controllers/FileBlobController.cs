using ImgriffStorage.Models.FileModels;
using ImgriffStorage.Services.AzureBlobStorageServices;
using ImgriffStorage.Services.HashServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImgriffStorage.Controllers
{
    [Route("imgriff-storage/blob")]
    [ApiController]
    public class FileBlobController(IBlobService blobService, IHashService hashService) : ControllerBase
    {
        private readonly IBlobService _blobService = blobService;
        private readonly IHashService _hashService = hashService;

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(FormFileModel fileModel)
        {
            if (fileModel == null || fileModel.formFile == null || 
                String.IsNullOrWhiteSpace(fileModel.UserEmail) || String.IsNullOrEmpty(fileModel.UserId))
            {
                return Ok(new { prhase = "model cannot be null" });
            }
            String bucketname = _hashService.HashString(fileModel.UserId + fileModel.UserEmail).Substring(0, 60);
            await _blobService.UploadFileAsync(bucketname, fileModel.formFile);
            return await GetUrl(fileModel.formFile.FileName, fileModel.UserEmail, fileModel.UserId);
        }

        [HttpGet("get-url")]
        public async Task<IActionResult> GetUrl(string fileName, string userEmail, string userId)
        {
            if (String.IsNullOrEmpty(fileName) ||
                String.IsNullOrWhiteSpace(userEmail) || String.IsNullOrEmpty(userId))
            {
                return Ok(new { prhase = "model cannot be null" });
            }

            String bucketname = _hashService.HashString(userId + userEmail).Substring(0, 60);
            var url = await _blobService.GetFileUrlAsync(bucketname, fileName);
            return Ok(new { url });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(FileModel fileModel)
        {
            if (fileModel == null || String.IsNullOrEmpty(fileModel.FileName) ||
               String.IsNullOrWhiteSpace(fileModel.UserEmail) || String.IsNullOrEmpty(fileModel.UserId))
            {
                return Ok(new { prhase = "model cannot be null" });
            }
            String bucketname = _hashService.HashString(fileModel.UserId + fileModel.UserEmail).Substring(0, 60);
            await _blobService.DeleteFileAsync(bucketname, fileModel.FileName);
            return Ok(new { });
        }

    }
}
