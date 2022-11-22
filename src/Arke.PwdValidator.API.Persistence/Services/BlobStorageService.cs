using Arke.PwdValidator.API.Application.Contracts.Infrastructure;
using Arke.PwdValidator.Shared.Exceptions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Arke.PwdValidator.API.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{

    private readonly IAppLogger<BlobStorageService> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IAppLogger<BlobStorageService> logger, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFile(string containerName, string fileName, byte[] data, bool allowOverwrite = true)
    {
        using (var stream = new MemoryStream(data))
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(fileName);
            if (await blob.ExistsAsync())
            {
                if (allowOverwrite)
                {
                    _logger.LogWarning($"{blob.Uri} already exists and will be overwritten.");
                }
                else
                {
                    var errorMessage = $"{blob.Uri} already exists and cannot be overwritten.";
                    _logger.LogWarning(errorMessage);
                    throw new BlobStorageException(errorMessage);
                }

            }
            await blob.UploadAsync(stream, allowOverwrite);
            return blob.Uri?.ToString();
        }
    }

    public async Task<bool> CheckFileExists(string containerName, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(fileName);
        return await blob.ExistsAsync();
    }

    public async Task<string> GetFileUri(string containerName, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(fileName);
        return blob.Uri?.ToString();
    }

    public async Task<byte[]> GetBlob(string containerName, string blobName, CancellationToken cancellationToken)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        BlobDownloadResult download = await blobClient.DownloadContentAsync(cancellationToken);

        return download.Content.ToMemory().ToArray();
    }

}
