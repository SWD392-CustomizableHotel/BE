using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SWD.SheritonHotel.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("certificatestaff");
        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
        await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });

        return blobClient.Uri.ToString();
    }

    public BlobContainerClient GetBlobContainerClient(string containerName)
    {
        return _blobServiceClient.GetBlobContainerClient(containerName);
    }
}
