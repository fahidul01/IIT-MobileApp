using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    public class FilesController : Controller
    {
        private readonly FileService _fileService;

        public FilesController(FileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(string id)
        {
            var file = await _fileService.GetFile(id);
            if (file != null && System.IO.File.Exists(file.FilePath))
            {
                var contentType = "APPLICATION/octet-stream";
                return File(System.IO.File.OpenRead(file.FilePath), contentType, file.FileName);
            }
            else
            {
                return NotFound();
            }
        }
    }
}