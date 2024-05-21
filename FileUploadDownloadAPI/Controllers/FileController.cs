using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FileUploadDownloadAPI.Models;

namespace FileUploadDownloadAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileIOContext _context;

        public FileController(FileIOContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult> GetFiles()
        {
            //return Ok("success");
            try
            {
                return Ok(await _context.TblFiles.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        private async Task<string> writeFile(IFormFile file)
        {
            string filename = "";
            try
            {
                filename = file.FileName;
                  var PathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");
                if (!Directory.Exists(PathBuilt))
                {
                    Directory.CreateDirectory(PathBuilt);
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch(Exception ex) 
            { 
            }
            return filename;
        }

        [HttpPost]
        [Route("SaveFile")]
        public async  Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var result = await writeFile(file);
            if (result != null)
            {
             var filetype =  file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string extension = System.IO.Path.GetExtension(file.FileName);
                string filename = file.FileName.Substring(0, file.FileName.Length - extension.Length);
                var PathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");
                TblFile fileDetail = new TblFile();
                fileDetail.FileName = filename;
                fileDetail.FileType = filetype;
                fileDetail.FilePath = PathBuilt;
                fileDetail.CreatedOn = DateTime.Now;
                _context.Add(fileDetail);
                _context.SaveChangesAsync();
            }       

          

            return Ok();
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> DownloadFile(int id)
        {

            var fileDetail = _context.TblFiles
               .Where(x => x.Id == id)
               .FirstOrDefault();
            if (fileDetail != null)
            {
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileDetail.FileName,
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                //get physical path
                var path = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");
                var fileReadPath = Path.Combine(path, fileDetail.FileName + "." + fileDetail.FileType);
                var fileReadvirtualPath = Path.Combine(path + "~" + fileDetail.FileName + "." + fileDetail.FileType);
                byte[] filepaths = System.IO.File.ReadAllBytes(fileReadPath);
                return File(filepaths, "application/octet-stream", fileReadvirtualPath);

            }
            else
            {
                return StatusCode(404);
            }
        }

      
    }
}
