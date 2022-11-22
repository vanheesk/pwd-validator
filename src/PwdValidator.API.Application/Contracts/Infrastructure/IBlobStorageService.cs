namespace Arke.PwdValidator.API.Application.Contracts.Infrastructure;

public interface IBlobStorageService
{

    Task<string> UploadFile(string containerName, string fileName, byte[] data, bool overwrite = true);

    Task<bool> CheckFileExists(string containerName, string fileName);

    Task<string> GetFileUri(string containerName, string fileName);

    Task<byte[]> GetBlob(string containerName, string blobName, CancellationToken cancellationToken);

}
