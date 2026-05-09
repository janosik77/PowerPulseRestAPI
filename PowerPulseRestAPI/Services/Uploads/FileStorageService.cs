using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PowerPulseRestAPI.Services.Uploads
{
    public class FileStorageService : IFileStorageService
    {
        private static readonly HashSet<string> AllowedCategories = new(StringComparer.OrdinalIgnoreCase)
        {
            "customer",
            "project",
            "employee",
            "knowledge",
            "vehicle",
            "material",
            "tool"
        };

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private const long MaxFileSizeInBytes = 5 * 1024 * 1024;
        private const int TargetWidth = 1200;
        private const int TargetHeight = 800;
        private const int JpegQuality = 85;

        private readonly IWebHostEnvironment _environment;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadAsync(
            IFormFile file,
            string category,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(file);

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new InvalidOperationException("Upload category is required.");
            }

            if (!AllowedCategories.Contains(category))
            {
                throw new InvalidOperationException($"Upload category '{category}' is not supported.");
            }

            if (file.Length == 0)
            {
                throw new InvalidOperationException("The file is empty.");
            }

            if (file.Length > MaxFileSizeInBytes)
            {
                throw new InvalidOperationException("The file is too large. Maximum allowed size is 5 MB.");
            }

            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Only .jpg, .jpeg, .png and .webp files are allowed.");
            }

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new InvalidOperationException("WebRootPath is not configured.");
            }

            var normalizedCategory = category.Trim().ToLowerInvariant();
            var uploadsFolder = Path.Combine(webRootPath, "uploads", normalizedCategory);

            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid():N}.jpg";
            var absolutePath = Path.Combine(uploadsFolder, storedFileName);

            await using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream, cancellationToken);

            image.Mutate(context =>
            {
                context.Resize(new ResizeOptions
                {
                    Size = new Size(TargetWidth, TargetHeight),
                    Mode = ResizeMode.Pad,
                    Position = AnchorPositionMode.Center
                });
            });

            var encoder = new JpegEncoder
            {
                Quality = JpegQuality
            };

            await image.SaveAsJpegAsync(absolutePath, encoder, cancellationToken);

            return $"/uploads/{normalizedCategory}/{storedFileName}";
        }

        public void DeleteFileByUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                return;
            }

            var normalizedUrl = url.Trim();

            if (!normalizedUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var relativePath = normalizedUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var absolutePath = Path.Combine(webRootPath, relativePath);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }
}