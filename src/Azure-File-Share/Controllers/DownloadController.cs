using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.IO;

namespace Azure_File_Share.Controllers
{
    public class DownloadController : Controller
    {
        public IActionResult Index(string fileUrl)
        {
            ViewData["Message"] = fileUrl;
            return View();
        }

        public FileResult Download(string fileUrl)
        {
            var fileName = $"img.jpg";
            var filepath = $"Downloads/{fileName}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }

        public FileResult DownloadMultipleFiles(string fileUrl)
        {
            // get file names by fileurl
            var fileName = $"img.jpg";
            var fileName2 = $"img2.jpg";
            string[] files = new string[] { fileName, fileName2 };
            byte[] compressedBytes;

            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    foreach (string fileSingle in files)
                    {
                        var fileInArchive = archive.CreateEntry(fileSingle, CompressionLevel.Optimal);
                        byte[] fileBytes = System.IO.File.ReadAllBytes($"Downloads/{fileSingle}");
                        using (var entryStream = fileInArchive.Open())
                        using (var fileToCompressStream = new MemoryStream(fileBytes))
                        {
                            fileToCompressStream.CopyTo(entryStream);
                        }
                    }
                }
                compressedBytes = outStream.ToArray();
            }
            return File(compressedBytes, "application/x-msdownload", "zipName.zip");
        }
    }
}
