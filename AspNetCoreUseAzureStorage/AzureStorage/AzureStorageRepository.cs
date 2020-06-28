using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCoreUseAzureStorage.AzureStorage
{
    public class AzureStorageRepository
    {
        private readonly AzureStorageOptions _azureStorageOptions;
        public AzureStorageRepository(IOptions<AzureStorageOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _azureStorageOptions = optionsAccessor.Value;
        }

        public async Task UploadAsync(ContainerName containerName, string blobName, Stream source)
        {
            var blobServiceClient = new BlobServiceClient(_azureStorageOptions.ConnectionString);

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName.ToString());

            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(source, new BlobHttpHeaders
            {
                //ContentType = "application/pdf"
            });
        }
    }
}
