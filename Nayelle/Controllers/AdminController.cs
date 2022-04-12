using Nayelle.Data.Repositories;
using Nayelle.Helpers;
using Nayelle.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nayelle.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/admin/SilkSerum");
        }

        public ActionResult SilkSerum()
        {
            var page = new SilkSerumVM(new SilkSerumPageRepo().SilkSerumPage);
            return View(page);
        }

        [HttpPost]
        public ActionResult Update()
        {
            return Redirect("/");
        }

        public bool CheckIfFileExists(HttpPostedFileBase uploadedfile, string folderPath)
        {
            var filename = folderPath + HttpUtility.UrlDecode(uploadedfile.FileName);
            if (System.IO.File.Exists(Server.MapPath(filename)))
            {
                return true;
            }
            return false;
        }

        public ResponseModel AttachFile(string ID)
        {
            var page = new SilkSerumPageRepo();
            var folder = SilkSerumPageRepo.ImagePath;
            HttpFileCollectionBase files = Request.Files;
            if (files == null || files.Count == 0)
            {
                return new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "Server error.",
                    Data = ""
                };
            }

            HttpPostedFileBase uploadedfile = files[0];
            var filename = Guid.NewGuid().ToString("N") + Path.GetExtension(uploadedfile.FileName);
            var fileDelete = string.Empty;
            if (ID == "hero")
            {
                fileDelete = page.SilkSerumPage.HeroImage;
            }
            else
            {
                fileDelete = page.SilkSerumPage.Products.FirstOrDefault(x => x.ID == ID)?.Picture;
            }

            if (ID == "hero")
            {
                page.SilkSerumPage.HeroImage = filename;
            }
            else if (page.SilkSerumPage.Products.FirstOrDefault(x => x.ID == ID) != null)
            {
                page.SilkSerumPage.Products.FirstOrDefault(x => x.ID == ID).Picture = filename;
            }
            else
            {
                return new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "Server error.",
                    Data = ""
                };
            }
            SystemFile.Update(folder + filename, uploadedfile.InputStream);
            if (!string.IsNullOrWhiteSpace(fileDelete))
            {
                SystemFile.Delete(folder + fileDelete);
            }

            return new ResponseModel()
            {
                IsSuccess = true,
                Message = "success",
                Data = filename
            };
        }
    }
}