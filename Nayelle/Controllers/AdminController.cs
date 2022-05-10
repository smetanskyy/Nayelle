using Nayelle.Data.Repositories;
using Nayelle.Helpers;
using Nayelle.Models;
using Nayelle.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["_loginTkn"] != null)
            {
                var accountService = new AccountService();
                var tkn = Request.Cookies["_loginTkn"].Value;
                var isValid = !string.IsNullOrWhiteSpace(tkn) && GuidToken.IsValidTokenBasedOnTime(tkn) && accountService.IsGuidValid(GuidToken.GetGuidFromToken(tkn));
                if (isValid)
                {
                    return RedirectToAction("SilkSerum");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult TakeScreenshot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TakeScreenshot(string url, int width = 1400, int height = 1750)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "http://54.219.72.121:6001/";
            }
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("incognito");
                //options.AddAdditionalCapability("resolution", "1920x1080", true);
                options.AddArgument("headless"); // Comment if we want to see the window.
                options.AddArgument("--hide-scrollbars");

                var pathApp = HttpRuntime.AppDomainAppPath;
                var pathDrive = Path.Combine(pathApp, "ChromeDriver");

                using (var driver = new ChromeDriver(pathDrive, options))
                {
                    string screenshotName = $"screenshot_{DateTime.Now.ToString("yyyy-MM-dd HH-mm")}.png";
                    //driver.Manage().Window.Maximize();
                    driver.Manage().Window.Size = new System.Drawing.Size(width, height);
                    driver.Navigate().GoToUrl(url);
                    var screenshot = (driver as ITakesScreenshot).GetScreenshot();
                    var pathImage = Path.Combine(pathApp, "Files", "Thumbnails", screenshotName);
                    screenshot.SaveAsFile(pathImage, ScreenshotImageFormat.Png);

                    //string screenshot64Str = screenshot.AsBase64EncodedString;
                    //screenshot64Str = screenshot.ToString(); //same as string screenshot = ss.AsBase64EncodedString;
                    //byte[] screenshotAsByteArray = screenshot.AsByteArray;

                    //Mobile
                    //var mobileName = Path.Combine(pathApp, "Files", "Thumbnails", "mob-" + screenshotName);
                    //driver.Manage().Window.Size = new System.Drawing.Size(360, 640);
                    //driver.Navigate().GoToUrl(url);
                    //screenshot = (driver as ITakesScreenshot).GetScreenshot();
                    //screenshot.SaveAsFile(mobileName, ScreenshotImageFormat.Png);

                    driver.Close();
                    driver.Quit();
                    return File(screenshot.AsByteArray, "application/octet-stream", screenshotName);
                }
            }
            catch (Exception ex)
            {
                return Content($"<p>{ex.Message}</p>");
            }
        }

        [HttpPost]
        public ActionResult Index(LoginVM login)
        {
            var isRememberMe = Request.Form["RememberMe"] ?? "";
            var accountService = new AccountService();
            var guid = accountService.GetGuidByCredentials(login.UserName, login.Password);
            if (string.IsNullOrWhiteSpace(guid))
            {
                return View();
            }
            var tkn = GuidToken.CreateTokenBasedOnGuid(guid, 24 * 3);
            var tknCookie = new HttpCookie("_loginTkn", tkn);
            tknCookie.Expires = DateTime.Now.Date.AddHours(3).AddDays(3);
            var rememberMe = new HttpCookie("_rememberMe", isRememberMe);
            rememberMe.Expires = DateTime.Now.Date.AddHours(3).AddDays(3);
            Response.Cookies.Set(tknCookie);
            Response.Cookies.Set(rememberMe);
            return RedirectToAction("SilkSerum");
        }

        public ActionResult SilkSerum()
        {
            if (Request.Cookies["_loginTkn"] != null)
            {
                var accountService = new AccountService();
                var tkn = Request.Cookies["_loginTkn"].Value;
                var isValid = !string.IsNullOrWhiteSpace(tkn) && GuidToken.IsValidTokenBasedOnTime(tkn) && accountService.IsGuidValid(GuidToken.GetGuidFromToken(tkn));

                if (!isValid || Request.Cookies["_rememberMe"] == null || Request.Cookies["_rememberMe"].Value != "on")
                {
                    var _loginTkn = Request.Cookies["_loginTkn"];
                    var _rememberMe = Request.Cookies["_rememberMe"];
                    _loginTkn.Expires = DateTime.Now.AddDays(-1);
                    _rememberMe.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Set(_loginTkn);
                    Response.Cookies.Set(_rememberMe);
                }
                if (!isValid)
                {
                    return RedirectToAction("Index");
                }
                var page = new SilkSerumVM(new SilkSerumPageRepo().SilkSerumPage);
                return View(page);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(SilkSerumVM model)
        {
            var isShowShadow = !string.IsNullOrWhiteSpace(Request.Form["IsShowShadow"]);
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
            silkserum.ParagraphUnderHeroFirst = model.ParagraphUnderHeroFirst;
            silkserum.ParagraphUnderHeroSecond = model.ParagraphUnderHeroSecond;
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
            silkserum.IsShowShadow = isShowShadow;

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