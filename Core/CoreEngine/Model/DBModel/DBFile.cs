﻿using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace CoreEngine.Model.DBModel
{
    public class DBFile
    {

        public int Id { get; set; }
        /// <summary>
        /// Can be duplicate
        public string FileName { get; set; }
        /// Should be unique
        /// </summary>
        public string FilePath { get; set; }
        public string FileHash { get; set; }
        public string Description { get; set; }
    }
}
