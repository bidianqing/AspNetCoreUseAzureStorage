using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AspNetCoreUseAzureStorage.AzureStorage
{
    public class AzureStorageRepository
    {
        private readonly AzureStorageOptions _azureStorageOptions;
        private readonly BlobServiceClient _blobServiceClient;
        public AzureStorageRepository(IOptions<AzureStorageOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _azureStorageOptions = optionsAccessor.Value;
            _blobServiceClient = new BlobServiceClient(_azureStorageOptions.ConnectionString);
        }

        public async Task UploadAsync(ContainerName containerName, string blobName, IFormFile file)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToString());

            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            var l = file.Length;
            Console.WriteLine($"文件大小{l}");
            using var content = file.OpenReadStream();
            await blobClient.UploadAsync(content, new BlobHttpHeaders
            {
                ContentType = file.ContentType
            }, null, null, new Progress(l));
        }
    }

    public class Progress : IProgress<long>
    {
        private long _length { get; set; }
        public Progress(long length)
        {
            _length = length;
        }

        public void Report(long value)
        {
            var ratio = value * 1.0 / _length * 100;

            Console.WriteLine(value + "进度为" + Math.Round(ratio, 1) + "%");
        }
    }
}
