using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class FileService : BaseService
    {
        private readonly StudentDBContext _db;

        public FileService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
        }

        public async Task<List<DBFile>> UploadFiles(IEnumerable<IFormFile> formFiles)
        {
            var files = new List<DBFile>();
            foreach (var file in formFiles.Where(x=>x.Length>0))
            {
                files.Add(await UploadFile(file));
            }
            return files;
        }

        public async Task<DBFile> UploadFile(IFormFile formFile)
        {
            var dir = AppConstants.DataPath;
            var target = Path.Combine("Uploads");

            var filePath = Path.GetTempFileName();
            using var stream = File.Create(filePath);
            await formFile.CopyToAsync(stream);

            var hash = HashFile(filePath);
            var oldFile = await _db.DBFiles
        }

        private string HashFile(string fileName)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(fileName);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
