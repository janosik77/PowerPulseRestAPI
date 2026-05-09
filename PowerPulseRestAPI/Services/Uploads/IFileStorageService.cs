
namespace PowerPulseRestAPI.Services.Uploads
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(
            IFormFile file,
            string category,
            CancellationToken cancellationToken = default);

        void DeleteFileByUrl(string? url);
    }
}
