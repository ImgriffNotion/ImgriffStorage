using System.Text.Json.Serialization;

namespace ImgriffStorage.Models.FileModels
{
    public class FormFileModel
    {
        public string? UserEmail { get; set; }
        public string? UserId { get; set; }
        public IFormFile? formFile { get; set; }
        public bool? IsGalleryImage { get; set; }

    }
}
