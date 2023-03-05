using FilesSystem.Data;
using FilesSystem.Models;
using FilesSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilesSystem.Controllers
{
    public class FoldersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFoldersService _service;
        public FoldersController(AppDbContext context, IFoldersService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFolders(int? parentId)
        {
            var folders = await _service.GetAllAsync(x=>x.ParentId == parentId);
            return View(folders);
        }
        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile zip)
        {
            if (zip == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _service.WriteZipHierarchyToDb(zip);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            return RedirectToAction("GetFolders");

        }
        [HttpGet]
        public IActionResult DownloadCsv()
        {
            return File(_service.GetBytesFromDb(), "text/csv", "Folders.csv");
        }
        [HttpGet]
        public async Task<IActionResult> AddCsv()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCsv(IFormFile csvFile)
        {
            await _service.ParseCsvAndAddToDb(csvFile);
            return RedirectToAction("GetFolders");

        }
    }
}
