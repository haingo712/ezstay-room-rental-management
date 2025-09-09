using Microsoft.AspNetCore.Http;

namespace ImageAPI.Validators
{
    public static class ImageValidator
    {
        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public static void Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File không hợp lệ");

            // Check mime type
            if (!AllowedMimeTypes.Contains(file.ContentType.ToLower()))
                throw new Exception("Chỉ được upload file ảnh (jpg, png, gif, webp)");

            // Check extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
                throw new Exception("Định dạng file không được phép");
        }
    }
}
