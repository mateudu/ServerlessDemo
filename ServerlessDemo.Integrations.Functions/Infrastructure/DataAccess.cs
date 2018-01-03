using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ServerlessDemo.Integrations.Functions.Abstract;
using ServerlessDemo.Web.Core.Model;

namespace ServerlessDemo.Integrations.Functions.Infrastructure
{
    public class DataAccess : IDataAccess
    {
        public DataAccess()
        {
            connString =  System.Environment.GetEnvironmentVariable(Consts.ConnectionStrings.DbConnectionString, EnvironmentVariableTarget.Process);
        }

        private readonly string connString;

        public async Task<Image> GetImage(int id)
        {
            using (IDbConnection con = new SqlConnection(connString))
            {
                var result = await con.QueryAsync<Image>("Select * From Images " +
                                                         "Where ImageId = @ImageId", 
                                                         new
                                                         {
                                                             ImageId = id
                                                         });
                return result.FirstOrDefault();
            }
        }

        public async Task<Image> GetImageByRelativePath(string relativePath)
        {
            using (IDbConnection con = new SqlConnection(connString))
            {
                var result = await con.QueryAsync<Image>("Select * From Images " +
                                                         "Where RelativePath = @RelativePath",
                    new
                    {
                        RelativePath = relativePath
                    });
                return result.FirstOrDefault();
            }
        }

        public async Task UpdateImageAllowedById(bool allowed, int id)
        {
            using (IDbConnection con = new SqlConnection(connString))
            {
                string updateQuery =
                    @"UPDATE [dbo].[Images] " +
                    @"SET Allowed = @Allowed WHERE ImageId = @ImageId";

                var result = await con.ExecuteAsync(updateQuery, new
                {
                    Allowed = allowed,
                    ImageId = id
                });
            }
        }

        public async Task UpdateImageThumbnailRelativePathById(string thumbnailRelativePath, int id)
        {
            using (IDbConnection con = new SqlConnection(connString))
            {
                string updateQuery =
                    @"UPDATE [dbo].[Images] " +
                    @"SET ThumbnailRelativePath = @ThumbnailRelativePath WHERE ImageId = @ImageId";

                var result = await con.ExecuteAsync(updateQuery, new
                {
                    ThumbnailRelativePath = thumbnailRelativePath,
                    ImageId = id
                });
            }
        }

        public Task<IList<Comment>> GetCommentsByImageId(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetCommentById(int commentId)
        {
            throw new NotImplementedException();
        }
    }
}
