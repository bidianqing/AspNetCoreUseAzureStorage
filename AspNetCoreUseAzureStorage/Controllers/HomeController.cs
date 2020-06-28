using AspNetCoreUseAzureStorage.AzureStorage;
using AspNetCoreUseAzureStorage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCoreUseAzureStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly AzureStorageRepository _azureStorageRepository;
        public HomeController(AzureStorageRepository azureStorageRepository)
        {
            _azureStorageRepository = azureStorageRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload(IFormFile file)
        {
            string blobName = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using (Stream stream = file.OpenReadStream())
            {
                await _azureStorageRepository.UploadAsync(ContainerName.avatar, blobName, stream);
            }

            return Json(new { success = true });
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
