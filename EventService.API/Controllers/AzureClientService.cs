using Azure.Storage.Blobs;

namespace EventService.API.Controllers {
    public class AzureClientService {
        private readonly string _containerName = "event-images";
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AzureClientService> _logger;
        public AzureClientService(BlobServiceClient blobServiceClient, ILogger<AzureClientService> logger) {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken) {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var id = Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient(id);
            _logger.LogInformation("Uploading Image {name} to Azure Storage", file.FileName);
            await using (var stream = file.OpenReadStream()) {
                await blobClient.UploadAsync(stream, cancellationToken);
            }
            return blobClient.Uri.ToString();
        }
    }
}
