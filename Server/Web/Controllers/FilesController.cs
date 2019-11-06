using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace Web.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly FileService _fileService;

        public FilesController(FileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var file = await _fileService.GetFile(id);
            if (file != null && System.IO.File.Exists(file.FilePath))
            {
                var contentType = "APPLICATION/octet-stream";
                return File(System.IO.File.OpenRead(file.FilePath), contentType, file.FileName);
            }
            else return NotFound();
        }
    }
}