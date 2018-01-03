using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServerlessDemo.Web.Core.Model;

namespace ServerlessDemo.Web.Core.Abstract
{
    public interface IDataAccess
    {
        Task<IList<Image>> GetAllImages();
        Task<IList<Image>> GetAllowedImages();
        Task<IList<Image>> GetPendingImages();
        Task<IList<Image>> GetBannedImages();
        Task<int> InsertImage(Image image);
        Task<Image> GetImage(int id);
        Task UpdateImageUploadedById(bool uploaded, int id);
        Task UpdateImageAllowedById(bool allowed, int id);
        Task<IList<Comment>> GetCommentsByImageId(int imageId);
        Task<Comment> GetCommentById(int commentId);
    }
}
