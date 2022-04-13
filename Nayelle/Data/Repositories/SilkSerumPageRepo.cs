using Nayelle.Helpers;
using Nayelle.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Nayelle.Data.Repositories
{
    public class SilkSerumPageRepo
    {
        public SilkSerumPage SilkSerumPage { get; set; }
        public static readonly string ImagePath = "/files/images/SilkSerum/";
        public static readonly string FilePath = "/app_data/content/SilkSerumPage.json";
        public SilkSerumPageRepo()
        {
            Refresh();
            if (string.IsNullOrWhiteSpace(SilkSerumPage.Title))
            {
                AddDefault();
            }
        }

        public void Refresh()
        {
            //add encoding in order to set correct font on the data
            if (!SystemFile.Exists(FilePath))
            {
                SystemFile.Update(FilePath, "");
            }

            string json = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(FilePath));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            SilkSerumPage = jss.Deserialize<SilkSerumPage>(json);
            SilkSerumPage = SilkSerumPage ?? new SilkSerumPage();
        }

        public void AddDefault()
        {
            var products = new List<SilkSerum>();
            products.Add(new SilkSerum()
            {
                ID = Guid.NewGuid().ToString("D"),
                Name = "SILK SERUMS BAKUCHOLI",
                Price = "$35",
                Ingredient = "BAKUCHOLI OIL",
                Property = "Retinol Alternative",
                Description = "A gentle, plant based retinol alternative Without irritation and safe to use daily. Seals in hydration and smooths the look of fine lines and wrinkles.",
                Picture = "SilkSerums_1.jpg"
            });

            products.Add(new SilkSerum()
            {
                ID = Guid.NewGuid().ToString("D"),
                Name = "SILK SERUMS HYALURONIC",
                Price = "$29",
                Ingredient = "HYALURONIC ACID 1%",
                Property = "Hydrating",
                Description = "A powerful hydrating must-have with 1% hyaluornic acid to hydrate the skin, leaving it smoother and plumper. Uses 3 molecular sizes of HA to deeply penetrate and hydrate the skin.",
                Picture = "SilkSerums_2.jpg"
            });

            products.Add(new SilkSerum()
            {
                ID = Guid.NewGuid().ToString("D"),
                Name = "SILK SERUMS VITAMIN C",
                Price = "$29",
                Ingredient = "VITAMIN C 5%",
                Property = "Brightening",
                Description = "A highly stable Vitamin C that delivers a potent dose of actives to help brighten dull, stressed-out skin while reducing hyperpigmentation and marks.",
                Picture = "SilkSerums_3.jpg"
            });

            products.Add(new SilkSerum()
            {
                ID = Guid.NewGuid().ToString("D"),
                Name = "SILK SERUMS NIACINAMIDE",
                Price = "$35",
                Ingredient = "NIACINAMIDE 4%",
                Property = "Anti-Aging & Repair",
                Description = "A supercharged anti-aging serum powered by 5% niacinamide a Natural vitaminB3. Increases collagen Production and combats fine lines, wrinkles.",
                Picture = "SilkSerums_4.jpg"
            });

            SilkSerumPage = new SilkSerumPage()
            {
                Title = "SILK SERUMS",
                HeroImage = "hero.jpeg",
                HeroTextTop = "100% Natural Serums for healthy, youthful, glowing skin.",
                HeroTextBottom = "* Repair * Protect * Hydrate * Brighten *",
                ParagraphUnderHero = "Why SILK SERUMS? All your skin needs in 4 little bottles. Powerful ingredients, fast acting, results driven.",
                ParagraphUnderProducts = "WHERE TO BUY?",
                LinkName = "NAYELLE Skincare",
                Link = "https://nayelle.com/",
                Advantage = "SILK SERUMS is a proudly Canadian made, family run business.",
                ContactUs = "To Contact Us:",
                Manufacture = "Manufactured for Silk Serums.",
                Street = "#300-West 14th Street",
                City = "North Vancouver, BC",
                PostalCode = "V7P 3P2",
                Email = "info@silkserums.com",
                Copyright = "Copyright © 2022 NAYELLE Probiotic Skincare | All Rights Reserved",
                Products = products
            };
            SaveJson(SilkSerumPage);
        }

        public void SaveJson(SilkSerumPage silkSerumPage)
        {
            //Create new file with json information
            var json = JsonConvert.SerializeObject(silkSerumPage);
            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(FilePath), json);
        }

        public bool FileDelete(string productID)
        {
            var product = SilkSerumPage.Products.FirstOrDefault(x => x.ID == productID);
            if (product == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(product.Picture))
            {
                product.Picture = "";
                SaveJson(SilkSerumPage);
                return true;
            }

            var path = ImagePath + product.Picture;

            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                SystemFile.Delete(path);
            }

            product.Picture = "";
            SaveJson(SilkSerumPage);
            return true;
        }

        public bool HeroDelete()
        {
            if (SilkSerumPage == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(SilkSerumPage.HeroImage))
            {
                SilkSerumPage.HeroImage = "";
                SaveJson(SilkSerumPage);
                return true;
            }

            var path = ImagePath + SilkSerumPage.HeroImage;

            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                SystemFile.Delete(path);
            }

            SilkSerumPage.HeroImage = "";
            SaveJson(SilkSerumPage);
            return true;
        }

        public string SavePicture(HttpPostedFileBase uploadedfile, string ID)
        {
            var page = new SilkSerumPageRepo();
            var folder = ImagePath;
            if (uploadedfile == null || string.IsNullOrWhiteSpace(uploadedfile.FileName))
            {
                return null;
            }

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

            SystemFile.Update(folder + filename, uploadedfile.InputStream);
            if (!string.IsNullOrWhiteSpace(fileDelete))
            {
                SystemFile.Delete(folder + fileDelete);
            }
            return filename;
        }
    }
}