using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Azure_File_Share.Helpers;
using Azure_File_Share.Core.Implementations;
using Microsoft.WindowsAzure.Storage;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Azure_File_Share.Controllers
{
    public class UploadController : Controller
    {
        CloudStorageAccount storageAccount;
        CommonFunctions comFunc = new CommonFunctions();

        public UploadController()
        {
            storageAccount = CloudStorageAccount.Parse(comFunc.GetAzureConfigString());
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            UserFileManager ufManager = new UserFileManager(storageAccount);
            //await ufManager.UploadFile(files[0]);
            string url = "";

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await files[0].CopyToAsync(stream);
                url = await ufManager.UploadFile(stream, files[0].FileName);
            }

            //foreach (var formFile in files)
            //{
            //    if (formFile.Length > 0)
            //    {
            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await formFile.CopyToAsync(stream);
            //            var ssre = stream;
            //        }
            //    }
            //}

            //return Ok(new { count = files.Count, size, filePath });
            //return RedirectToAction("Index", "Home/UploadSuccess", new { fileUrl = "adsdasdasd" });

            return RedirectToAction("UploadSuccess", "Home", new { fileUrl = string.Format("{0}?fileUrl={1}", Url.Action("Download", "Download"),url) });
            //return View("~/Views/Home/UploadSuccess.cshtml", "link-guug-g-g-g");
            //return Json(new { hasError = true}); // return file url
        }

        public IActionResult UploadSuccess(string fileUrl)
        {
            ViewData["Message"] = fileUrl;
            return View();
        }
    }
}
