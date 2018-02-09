using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCoreUseAzureStorage.AzureStorage
{
    public class AzureStorageRepository
    {
        private readonly AzureStorageOptions _azureStorageOptions;
        private static CloudStorageAccount _cloudStorageAccount;
        private static CloudBlobClient _cloudBlobClient;
        private static object _obj = new object();
        public AzureStorageRepository(IOptions<AzureStorageOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _azureStorageOptions = optionsAccessor.Value;

            if (_cloudStorageAccount == null)
            {
                lock (_obj)
                {
                    if (_cloudStorageAccount == null)
                    {
                        _cloudStorageAccount = CloudStorageAccount.Parse(_azureStorageOptions.ConnectionString);
                        _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
                    }
                }
            }
        }

        private async ValueTask<CloudBlockBlob> GetBlobReferenceAsync(ContainerName containerName, string blobName)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName.ToString().ToLower());
            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            return blockBlob;
        }

        public async Task UploadFromStreamAsync(ContainerName containerName, string blobName, Stream source)
        {
            CloudBlockBlob blockBlob = await GetBlobReferenceAsync(containerName, blobName);
            await blockBlob.UploadFromStreamAsync(source);
        }

        public async Task<bool> DeleteIfExistsAsync(ContainerName containerName, string blobName)
        {
            CloudBlockBlob blockBlob = await GetBlobReferenceAsync(containerName, blobName);
            return await blockBlob.DeleteIfExistsAsync();
        }

    }
}
