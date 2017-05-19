using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Azure_File_Share.Helpers;
using Azure_File_Share.Core.Implementations;
using System.Threading.Tasks;

namespace Azure_File_Share.Controllers
{
    public class DownloadController : Controller
    {
        CloudStorageAccount storageAccount;
        CommonFunctions comFunc = new CommonFunctions();
        public DownloadController()
        {
            storageAccount = CloudStorageAccount.Parse(comFunc.GetAzureConfigString());
        }

        public IActionResult Index(string fileUrl)
        {
            ViewData["Message"] = fileUrl;
            return View();
        }

        public async Task<FileResult> Download(string fileUrl)
        {
            UserFileManager ufManager = new UserFileManager(storageAccount);
            var fileName = ufManager.GetFileNameFromUrl(fileUrl);
            byte[] fileBytes = await ufManager.DownloadFile(fileUrl);
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
