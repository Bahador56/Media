using Media.Data;
using Media.Models;
using Media.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace Media.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db,IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            var folderList = _db.Folders.Where(x => x.ParentId == null).ToList();

            return View(folderList);
        }


        [HttpGet]
        public IActionResult AddNewFolder(int? parentId)
        {
            ViewBag.ParentId = parentId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewFolder(Folder model)
        {
            if (ModelState.IsValid)
            {
                if (_db.Folders.Any(x => x.ParentId == model.ParentId && x.Title == model.Title))
                {
                    var list = _db.Folders.Where(x => x.ParentId == model.ParentId && x.Title == model.Title).ToList();
                    foreach (var item in list)
                    {
                        if (item.Title == model.Title)
                        {
                            ModelState.AddModelError("Entity Error", "Title and parent Folder is not unique");
                            return View(model);
                        }
                    }
                }

                _db.Folders.Add(model);
                var saveResult = _db.SaveChanges();
                if (saveResult > 0)
                    if (model.ParentId == null)
                        return RedirectToAction(nameof(Index));
                    else
                        return Redirect($"/home/folder?folderId={model.ParentId}");
            }

            return View(model);

        }


        //[Route("{folderId:int}")]
        //[HttpGet("{folderId:int}/{parentId:int}")]
        public IActionResult Folder(int folderId)
        {
            var folder = _db.Folders
                .Include(x => x.Files)
                .Include(x => x.Childs)
                .FirstOrDefault(x => x.Id == folderId);
            return View(folder);
        }


        [HttpPost]
        public IActionResult RenameFolder(string folderTitle, int folderId)
        {
            var folders = _db.Folders.Where(x => x.Title == folderTitle).ToList();
            foreach (var item in folders)
            {
                if (item.Title == folderTitle)
                {
                    return Json(new { isSuccess = false, message = "Title used by another folder!" });
                }
            }
            var folder = _db.Folders.FirstOrDefault(x => x.Id == folderId);
            folder.Title = folderTitle;
            var saveResult = _db.SaveChanges();
            if (saveResult > 0)
                return Json(new { isSuccess = true, message = "Successfully changed" });
            return Json(new { isSuccess = false, message = "Error in saveing change happend!" });
        }

        public IActionResult Uploadfile(IList<IFormFile> files,int? folderId)
        {
            foreach (var formFile in files)
            {
                var rootPath = _env.WebRootPath;
                try
                {
                    if (formFile.Length > 0)
                    {
                        var filenameFerfix = DateTime.Now.Ticks.ToString();
                        var filePath = Path.Combine(@$"{rootPath}\Files", filenameFerfix + formFile.FileName);
                        long size = 0;
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            formFile.CopyTo(stream);
                            size=stream.Length;
                        }
                        var  fileTypeId = 0;

                        switch(Path.GetExtension(formFile.FileName))
                        {
                            case ".txt":
                                fileTypeId = 1;
                                break;
                            case ".jpg":
                            case ".png":
                            case ".gif":
                                fileTypeId = 2;
                                break;
                            case ".mp4":
                            case ".mkv":
                            case ".3gp":
                                fileTypeId = 3;
                                break;
                            case ".mp3":
                            case ".acc":
                                fileTypeId = 4;
                                break;

                            default:
                                throw new Exception("File Format not acceptable");
                        }


                        var fileIndb = new Media.Models.Entity.File
                        {
                            Title = formFile.FileName,
                            Caption = "",
                            CreateAt = DateTime.Now,
                            Description = "",
                           FolderId= folderId,
                           FileTypeId= fileTypeId,
                           Format=Path.GetExtension(formFile.FileName),
                           Path= @$"{rootPath}\Files",
                           Size=Convert.ToInt32(size),
                           FileNameOnStarage= filenameFerfix + formFile.FileName

                        };
                        _db.Files.Add(fileIndb);
                        _db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            } 
            return Json(true);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}