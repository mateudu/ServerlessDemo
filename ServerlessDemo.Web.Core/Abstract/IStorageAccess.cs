using System.Threading.Tasks;
using ServerlessDemo.Web.Core.Model;

namespace ServerlessDemo.Web.Core.Abstract
{
    public interface IStorageAccess
    {
        BlobPathResponse GetPathForPendingImage(string filename);
        string GetUrlForRelativePath(string relativePath);
        Task MoveImage(BlobMoveRequest req);
        Task<BlobUploadResponse> UploadPendingImage(BlobUploadRequest req);
    }
}
