using Microsoft.Extensions.Options;

namespace AspNetCoreUseAzureStorage.AzureStorage
{
    public class AzureStorageOptions : IOptions<AzureStorageOptions>
    {
        public string ConnectionString { get; set; }

        public AzureStorageOptions Value => this;
    }
}
