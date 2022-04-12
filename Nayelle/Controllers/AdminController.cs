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
        public ActionResult Update(SilkSerumVM model)
        {
            var repo = new SilkSerumPageRepo();
            var silkserum = repo.SilkSerumPage;
            silkserum.Products[0].ID = model.ID_1;
            silkserum.Products[0].Name = model.Name_1;
            silkserum.Products[0].Price = model.Price_1;
            silkserum.Products[0].Description = model.Description_1;
            silkserum.Products[0].Ingredient = model.Ingredient_1;
            silkserum.Products[0].Property = model.Property_1;

            silkserum.Products[1].ID = model.ID_2;
            silkserum.Products[1].Name = model.Name_2;
            silkserum.Products[1].Price = model.Price_2;
            silkserum.Products[1].Description = model.Description_2;
            silkserum.Products[1].Ingredient = model.Ingredient_2;
            silkserum.Products[1].Property = model.Property_2;

            silkserum.Products[2].ID = model.ID_3;
            silkserum.Products[2].Name = model.Name_3;
            silkserum.Products[2].Price = model.Price_3;
            silkserum.Products[2].Description = model.Description_3;
            silkserum.Products[2].Ingredient = model.Ingredient_3;
            silkserum.Products[2].Property = model.Property_3;

            silkserum.Products[3].ID = model.ID_4;
            silkserum.Products[3].Name = model.Name_4;
            silkserum.Products[3].Price = model.Price_4;
            silkserum.Products[3].Description = model.Description_4;
            silkserum.Products[3].Ingredient = model.Ingredient_4;
            silkserum.Products[3].Property = model.Property_4;

            silkserum.Title = model.Title;
            silkserum.HeroTextTop = model.HeroTextTop;
            silkserum.HeroTextBottom = model.HeroTextBottom;
            silkserum.ParagraphUnderHero = model.ParagraphUnderHero;
            silkserum.ParagraphUnderProducts = model.ParagraphUnderProducts;
            silkserum.Link = model.Link;
            silkserum.LinkName = model.LinkName;
            silkserum.Advantage = model.Advantage;
            silkserum.Email = model.Email;
            silkserum.ContactUs = model.ContactUs;
            silkserum.Manufacture = model.Manufacture;
            silkserum.Street = model.Street;
            silkserum.City = model.City;
            silkserum.PostalCode = model.PostalCode;
            silkserum.Copyright = model.Copyright;

            var filename = repo.SavePicture(model.PictureHero, "hero");
            if (filename != null) silkserum.HeroImage = filename;
            filename = repo.SavePicture(model.Picture_1, model.ID_1);
            if (filename != null) silkserum.Products[0].Picture = filename;
            filename = repo.SavePicture(model.Picture_2, model.ID_2);
            if (filename != null) silkserum.Products[1].Picture = filename;
            filename = repo.SavePicture(model.Picture_3, model.ID_3);
            if (filename != null) silkserum.Products[2].Picture = filename;
            filename = repo.SavePicture(model.Picture_4, model.ID_4);
            if (filename != null) silkserum.Products[3].Picture = filename;
            repo.SaveJson(silkserum);
            return Redirect("/admin/SilkSerum");
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