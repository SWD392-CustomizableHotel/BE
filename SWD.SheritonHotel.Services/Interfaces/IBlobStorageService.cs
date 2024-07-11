using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        BlobContainerClient GetBlobContainerClient(string containerName);
    }
}
