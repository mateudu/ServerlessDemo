using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ServerlessDemo.Web.Core.Abstract;
using ServerlessDemo.Web.Core.Model;

namespace ServerlessDemo.Web.Core.Infrastructure
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration _configuration;

        public DataAccess(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        public async Task<IList<Image>> GetAllImages()
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
            using (IDbConnection con = new SqlConnection(connString))
            {
                var result = await con.QueryAsync<Image>("Select * From Images");
                return result.ToList();
            }
        }

        public async Task<IList<Image>> GetAllowedImages()
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
            using (IDbConnection con = new SqlConnection(connString))
            {
                var result = await con.QueryAsync<Image>("Select * From Images " +
                                                         "Where Uploaded = 1 And Allowed = 1");
                return result.ToList();
            }
        }

        public async Task<IList<Image>> GetPendingImages()
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
            using (IDbConnection con = new SqlConnection(connString))
            {
                var result = await con.QueryAsync<Image>("Select * From Images " +
                                                         "Where Uploaded = 1 And Allowed Is Null");
                return result.ToList();
            }
        }

        public async Task<IList<Image>> GetBannedImages()
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
            using (IDbConnection con = new SqlConnection(connString))
            {
                var result = await con.QueryAsync<Image>("Select * From Images " +
                                                         "Where Uploaded = 1 And Allowed = 0");
                return result.ToList();
            }
        }

        public async Task<int> InsertImage(Image image)
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
            using (IDbConnection con = new SqlConnection(connString))
            {
                string insertQuery = 
                    @"INSERT INTO [dbo].[Images]([RelativePath], [ThumbnailRelativePath], [OwnerUpn], [AddedAt], [Uploaded], [Allowed]) " +
                    @"VALUES (@RelativePath, @ThumbnailRelativePath, @OwnerUpn, @AddedAt, @Uploaded, @Allowed); " +
                    @"SELECT CAST(SCOPE_IDENTITY() as int)";

                var result = await con.QueryAsync<int>(insertQuery, new
                {
                    image.RelativePath,
                    image.ThumbnailRelativePath,
                    image.OwnerUpn,
                    image.AddedAt,
                    image.Uploaded,
                    image.Allowed
                });
                return result.Single();
            }
        }

        public async Task<Image> GetImage(int id)
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
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

        public async Task UpdateImageUploadedById(bool uploaded, int id)
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
            using (IDbConnection con = new SqlConnection(connString))
            {
                string updateQuery = 
                    @"UPDATE [dbo].[Images] "+
                    @"SET Uploaded = @Uploaded WHERE ImageId = @ImageId";

                var result = await con.ExecuteAsync(updateQuery, new
                {
                    Uploaded = uploaded,
                    ImageId = id
                });
            }
        }

        public async Task UpdateImageAllowedById(bool allowed, int id)
        {
            var connString = _configuration[Consts.ConnectionStrings.DbConnectionString];
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
