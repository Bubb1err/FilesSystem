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
            try 
            {
                var folders = await _service.GetAllAsync(x => x.ParentId == parentId);
                return View(folders);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }                 
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
            try
            {
                return File(_service.GetBytesFromDb(), "text/csv", "Folders.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> AddCsv()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCsv(IFormFile csvFile)
        {
            try
            {
                await _service.ParseCsvAndAddToDb(csvFile);
                return RedirectToAction("GetFolders");
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
            

        }
    }
}
