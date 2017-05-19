using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Azure_File_Share.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Upload()
        {
            ViewData["Message"] = "You can upload files here";
            RedirectToActionResult redirectResult = new RedirectToActionResult("Index", "Upload", null);
            return redirectResult;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult UploadSuccess(string fileUrl)
        {
            RedirectToActionResult redirectResult = new RedirectToActionResult("UploadSuccess", "Upload", new { fileUrl = fileUrl });
            return redirectResult;
        }
        public IActionResult Download()
        {
            return View();
        }
        public IActionResult Download(string fileUrl)
        {
            RedirectToActionResult redirectResult = new RedirectToActionResult("Download", "Download", new { fileUrl = fileUrl });
            return redirectResult;
        }
    }
}
