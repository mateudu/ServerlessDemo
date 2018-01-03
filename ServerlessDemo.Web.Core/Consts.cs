namespace ServerlessDemo.Web.Core
{
    public static class Consts
    {
        public static class ConnectionStrings
        {
            public const string DbConnectionString = "ConnectionStrings:ServerlessDemoDbConnectionString";
            public const string StorageConnectionString = "ConnectionStrings:ServerlessDemoStorageConnectionString";
        }

        public static class AzureAd
        {
            public const string ApplicationId = "AzureAd:ClientId";
            public const string TenantId = "AzureAd:TenantId";
            public const string IssuerInstance = "AzureAd:IssuerInstance";
            public const string Secret = "AzureAd:Secret";
            public const string Tenant = "AzureAd:Tenant";
            public const string Instance = "AzureAd:Instance";
        }

        public static class Groups
        {
            public const string AdminGroup = "Groups:AdminGroup";
        }

        public static class Storage
        {
            public const string AllowedImagesContainer = "Storage:AllowedImagesContainer";
            public const string PendingImagesContainer = "Storage:PendingImagesContainer";
            public const string BannedImagesContainer = "Storage:BannedImagesContainer";
            public const string ThumbnailImagesContainer = "Storage:ThumbnailImagesContainer";
        }

        public static class Claims
        {
            public const string Upn = @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
            public const string Sub = @"http://schemas.microsoft.com/identity/claims/objectidentifier";
        }
    }
}
