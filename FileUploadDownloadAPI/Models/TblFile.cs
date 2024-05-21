using System;
using System.Collections.Generic;

namespace FileUploadDownloadAPI.Models
{
    public partial class TblFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
