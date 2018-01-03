using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ServerlessDemo.Web.Core.Abstract;
using ServerlessDemo.Web.Core.Model;

namespace ServerlessDemo.Web.Core.Infrastructure
{
    public class StorageAccess : IStorageAccess
    {
        private readonly IConfiguration _configuration;

        public StorageAccess(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        public BlobPathResponse GetPathForPendingImage(string filename)
        {
            var account = CloudStorageAccount.Parse(_configuration[Consts.ConnectionStrings.StorageConnectionString]);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_configuration[Consts.Storage.PendingImagesContainer]);

            var blob = container.GetBlockBlobReference(filename);

            return new BlobPathResponse()
            {
                RelativePath = blob.Uri.AbsolutePath,
                Url = blob.Uri.ToString()
            };
        }

        public BlobPathResponse GetPathForBannedImage(string filename)
        {
            var account = CloudStorageAccount.Parse(_configuration[Consts.ConnectionStrings.StorageConnectionString]);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_configuration[Consts.Storage.BannedImagesContainer]);

            var blob = container.GetBlockBlobReference(filename);

            return new BlobPathResponse()
            {
                RelativePath = blob.Uri.AbsolutePath,
                Url = blob.Uri.ToString()
            };
        }

        public BlobPathResponse GetPathForAllowedImage(string filename)
        {
            var account = CloudStorageAccount.Parse(_configuration[Consts.ConnectionStrings.StorageConnectionString]);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_configuration[Consts.Storage.AllowedImagesContainer]);

            var blob = container.GetBlockBlobReference(filename);

            return new BlobPathResponse()
            {
                RelativePath = blob.Uri.AbsolutePath,
                Url = blob.Uri.ToString()
            };
        }

        public string GetUrlForRelativePath(string relativePath)
        {
            var account = CloudStorageAccount.Parse(_configuration[Consts.ConnectionStrings.StorageConnectionString]);
            var blobClient = account.CreateCloudBlobClient();
            return new Uri(blobClient.BaseUri, relativePath).ToString();
        }

        public async Task<BlobUploadResponse> UploadPendingImage(BlobUploadRequest req)
        {
            var account = CloudStorageAccount.Parse(_configuration[Consts.ConnectionStrings.StorageConnectionString]);
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_configuration[Consts.Storage.PendingImagesContainer]);
            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), new OperationContext());

            var blob = container.GetBlockBlobReference(req.Name);
            await blob.UploadFromByteArrayAsync(req.Bytes, 0, req.Bytes.Length);

            blob.Properties.ContentType = req.ContentType;
            await blob.SetPropertiesAsync();

            return new BlobUploadResponse()
            {
                Name = blob.Name,
                Url = blob.Uri.ToString(),
                RelativePath = blob.Uri.AbsolutePath
            };
        }

        public async Task MoveImage(BlobMoveRequest req)
        {
            var account = CloudStorageAccount.Parse(_configuration[Consts.ConnectionStrings.StorageConnectionString]);
            var blobClient = account.CreateCloudBlobClient();

            var srcContainerName = req.SourceRelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
            var destContainerName = req.DestinationRelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];

            await blobClient.GetContainerReference(destContainerName)
                .CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), new OperationContext());

            var srcFileName = req.SourceRelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            var destFileName = req.DestinationRelativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];

            var srcBlob = blobClient.GetContainerReference(srcContainerName).GetBlockBlobReference(srcFileName);
            var destBlob = blobClient.GetContainerReference(destContainerName).GetBlockBlobReference(destFileName);

            await destBlob.StartCopyAsync(srcBlob);
            await srcBlob.DeleteAsync();
        }
    }
}
