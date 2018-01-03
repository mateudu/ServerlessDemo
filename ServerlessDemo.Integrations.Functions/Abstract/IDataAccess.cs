using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessDemo.Web.Core.Model;

namespace ServerlessDemo.Integrations.Functions.Abstract
{
    public interface IDataAccess
    {
        Task<Image> GetImage(int id);
        Task<Image> GetImageByRelativePath(string relativePath);
        Task UpdateImageAllowedById(bool allowed, int id);
        Task UpdateImageThumbnailRelativePathById(string thumbnailRelativePath, int id);
        Task<IList<Comment>> GetCommentsByImageId(int imageId);
        Task<Comment> GetCommentById(int commentId);
    }
}
