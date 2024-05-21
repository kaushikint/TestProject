namespace FileUploadDownload.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public IFormFile FormFile { get; set; } = null!;
    }
}
