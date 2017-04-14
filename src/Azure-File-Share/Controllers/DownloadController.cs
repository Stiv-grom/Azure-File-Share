using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
    }
}
