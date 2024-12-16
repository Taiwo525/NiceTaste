namespace LocalServicesMarketplace.Identity.API.Infrastructure.Storage
{
    public class StorageSettings
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountKey { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
        public int MaxFileSizeMB { get; set; } = 5;
        public List<string> AllowedFileTypes { get; set; } = new();
    }
}
