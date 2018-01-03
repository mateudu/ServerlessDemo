using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerlessDemo.Web.Core;
using ServerlessDemo.Web.Core.Abstract;
using ServerlessDemo.Web.Core.Model;
using ServerlessDemo.Web.Service.ViewModels;

namespace ServerlessDemo.Web.Service.Controllers
{
    [Route("images")]
    public class ImagesController : Controller
    {
        private readonly IStorageAccess _storageAccess;
        private readonly IDataAccess _dataAccess;
        private readonly IAadHelper _aadHelper;

        public ImagesController(
            IStorageAccess storageAccess,
            IDataAccess dataAccess,
            IAadHelper aadHelper
        )
        {
            _storageAccess = storageAccess;
            _dataAccess = dataAccess;
            _aadHelper = aadHelper;
        }

        [HttpGet("")]
        public async Task<IActionResult> AllowedImages()
        {
            var images = await _dataAccess.GetAllowedImages();

            var vm = new ImageListViewModel()
            {
                Images = images?.Select (x => new ImageListViewModel.Image()
                {
                    Id = x.ImageId ?? 0,
                    Url = _storageAccess.GetUrlForRelativePath(x.RelativePath),
                    ThumbnailUrl = string.IsNullOrWhiteSpace(x.ThumbnailRelativePath) ? null : _storageAccess.GetUrlForRelativePath(x.ThumbnailRelativePath)
                }).ToList()
            };
            return View(vm);
        }

        [HttpGet("banned")]
        public async Task<IActionResult> BannedImages()
        {
            var claims = User.Claims.ToList();
            string subId = claims.FirstOrDefault(x => x.Type == Consts.Claims.Sub)?.Value;

            if (!await _aadHelper.IsAdminBySub(subId))
            {
                return Unauthorized();
            }

            var images = await _dataAccess.GetBannedImages();

            var vm = new ImageListViewModel()
            {
                Images = images?.Select(x => new ImageListViewModel.Image()
                {
                    Id = x.ImageId ?? 0,
                    Url = _storageAccess.GetUrlForRelativePath(x.RelativePath),
                    ThumbnailUrl = string.IsNullOrWhiteSpace(x.ThumbnailRelativePath) ? null : _storageAccess.GetUrlForRelativePath(x.ThumbnailRelativePath)
                }).ToList()
            };
            return View(vm);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> PendingImages()
        {
            var claims = User.Claims.ToList();
            string subId = claims.FirstOrDefault(x => x.Type == Consts.Claims.Sub)?.Value;

            if (!await _aadHelper.IsAdminBySub(subId))
            {
                return Unauthorized();
            }

            var images = await _dataAccess.GetPendingImages();

            var vm = new ImageListViewModel()
            {
                Images = images?.Select(x => new ImageListViewModel.Image()
                {
                    Id = x.ImageId ?? 0,
                    Url = _storageAccess.GetUrlForRelativePath(x.RelativePath),
                    ThumbnailUrl = string.IsNullOrWhiteSpace(x.ThumbnailRelativePath) ? null : _storageAccess.GetUrlForRelativePath(x.ThumbnailRelativePath)
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost("")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var uploadRequest = new BlobUploadRequest()
            {
                ContentType = file.ContentType,
                Name = Guid.NewGuid().ToString()
            };

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    uploadRequest.Bytes = ms.ToArray();
                }
            }
            else
            {
                return BadRequest("Empty file");
            }

            var upn = User.Claims.FirstOrDefault(x => x.Type == Consts.Claims.Upn)?.Value;

            // Get paths for storage
            var paths = _storageAccess.GetPathForPendingImage(uploadRequest.Name);

            // Create image in DB
            int id = await _dataAccess.InsertImage(new Image()
            {
                AddedAt = DateTime.UtcNow,
                Allowed = null,
                OwnerUpn = upn,
                RelativePath = paths.RelativePath,
                ThumbnailRelativePath = null,
                Uploaded = false
            });
            
            // Upload image
            var uploadResponse = await _storageAccess.UploadPendingImage(uploadRequest);

            // Flag image as uploaded in DB
            await _dataAccess.UpdateImageUploadedById(true, id);

            return RedirectToAction(nameof(ImageDetails), new { id = id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ImageDetails(int id)
        {
            var image = await _dataAccess.GetImage(id);
            if (image == null)
            {
                return NotFound("Image not found");
            }

            var claims = User.Claims.ToList();
            string subId = claims.FirstOrDefault(x => x.Type == Consts.Claims.Sub)?.Value;

            bool isAdmin = await _aadHelper.IsAdminBySub(subId);

            var vm = new ImageDetailsViewModel()
            {
                Id = image.ImageId ?? 0,
                IsAdmin = isAdmin,
                ThumbnailUrl = string.IsNullOrWhiteSpace(image.ThumbnailRelativePath) ? null : _storageAccess.GetUrlForRelativePath(image.ThumbnailRelativePath),
                Url = _storageAccess.GetUrlForRelativePath(image.RelativePath),
                Allowed = image.Allowed
            };
            return View(vm);
        }

        [HttpPost("{id}/allow")]
        public async Task<IActionResult> AllowImage(int id)
        {
            var claims = User.Claims.ToList();
            string subId = claims.FirstOrDefault(x => x.Type == Consts.Claims.Sub)?.Value;

            if (!await _aadHelper.IsAdminBySub(subId))
            {
                return Unauthorized();
            }

            await _dataAccess.UpdateImageAllowedById(true, id);

            return RedirectToAction(nameof(ImageDetails), new { id = id });
        }

        [HttpPost("{id}/ban")]
        public async Task<IActionResult> BanImage(int id)
        {
            var claims = User.Claims.ToList();
            string subId = claims.FirstOrDefault(x => x.Type == Consts.Claims.Sub)?.Value;

            if (!await _aadHelper.IsAdminBySub(subId))
            {
                return Unauthorized();
            }

            await _dataAccess.UpdateImageAllowedById(false, id);

            return RedirectToAction(nameof(ImageDetails), new { id = id });
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> PostComment(Guid id)
        {
            return View();
        }
    }
}
