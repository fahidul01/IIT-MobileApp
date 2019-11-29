using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface IFilePicker
    {
        Task<string> PickImageFromGallery();
        Task<string> PickImageFromCamera();
        Task<string> PickFile();
    }
}
