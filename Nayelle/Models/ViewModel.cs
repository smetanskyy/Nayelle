using Nayelle.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Nayelle.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class LoginVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SilkSerumVM
    {
        // product
        public string ID_1 { get; set; }
        public string Name_1 { get; set; }
        public string Price_1 { get; set; }
        public string Description_1 { get; set; }
        public string Ingredient_1 { get; set; }
        public string Property_1 { get; set; }
        [NotMapped]
        public HttpPostedFileBase Picture_1 { get; set; }
        public string FileName_1 { get; set; }


        public string ID_2 { get; set; }
        public string Name_2 { get; set; }
        public string Price_2 { get; set; }
        public string Description_2 { get; set; }
        public string Ingredient_2 { get; set; }
        public string Property_2 { get; set; }
        [NotMapped]
        public HttpPostedFileBase Picture_2 { get; set; }
        public string FileName_2 { get; set; }


        public string ID_3 { get; set; }
        public string Name_3 { get; set; }
        public string Price_3 { get; set; }
        public string Description_3 { get; set; }
        public string Ingredient_3 { get; set; }
        public string Property_3 { get; set; }
        [NotMapped]
        public HttpPostedFileBase Picture_3 { get; set; }
        public string FileName_3 { get; set; }


        public string ID_4 { get; set; }
        public string Name_4 { get; set; }
        public string Price_4 { get; set; }
        public string Description_4 { get; set; }
        public string Ingredient_4 { get; set; }
        public string Property_4 { get; set; }
        [NotMapped]
        public HttpPostedFileBase Picture_4 { get; set; }
        public string FileName_4 { get; set; }

        // page
        public string Title { get; set; }
        public string HeroTextTop { get; set; }
        public string HeroTextBottom { get; set; }
        [NotMapped]
        public HttpPostedFileBase PictureHero { get; set; }
        public string HeroImage { get; set; }
        public string ParagraphUnderHeroFirst { get; set; }
        public string ParagraphUnderHeroSecond { get; set; }
        public string ParagraphUnderProducts { get; set; }
        public string Link { get; set; }
        public string LinkName { get; set; }
        public string Advantage { get; set; }
        public string Email { get; set; }
        public string ContactUs { get; set; }
        public string Manufacture { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Copyright { get; set; }
        public bool IsShowShadow { get; set; }

        public SilkSerumVM() { }
        public SilkSerumVM(SilkSerumPage model)
        {
            string folder = SilkSerumPageRepo.ImagePath;
            if (model.Products != null && model.Products.Count() == 4)
            {
                ID_1 = model.Products[0].ID;
                Name_1 = model.Products[0].Name;
                Price_1 = model.Products[0].Price;
                Description_1 = model.Products[0].Description;
                Ingredient_1 = model.Products[0].Ingredient;
                Property_1 = model.Products[0].Property;
                FileName_1 = $"{folder}{model.Products[0].Picture}";

                ID_2 = model.Products[1].ID;
                Name_2 = model.Products[1].Name;
                Price_2 = model.Products[1].Price;
                Description_2 = model.Products[1].Description;
                Ingredient_2 = model.Products[1].Ingredient;
                Property_2 = model.Products[1].Property;
                FileName_2 = $"{folder}{model.Products[1].Picture}";

                ID_3 = model.Products[2].ID;
                Name_3 = model.Products[2].Name;
                Price_3 = model.Products[2].Price;
                Description_3 = model.Products[2].Description;
                Ingredient_3 = model.Products[2].Ingredient;
                Property_3 = model.Products[2].Property;
                FileName_3 = $"{folder}{model.Products[2].Picture}";

                ID_4 = model.Products[3].ID;
                Name_4 = model.Products[3].Name;
                Price_4 = model.Products[3].Price;
                Description_4 = model.Products[3].Description;
                Ingredient_4 = model.Products[3].Ingredient;
                Property_4 = model.Products[3].Property;
                FileName_4 = $"{folder}{model.Products[3].Picture}";
            }

            Title = model.Title;
            HeroTextTop = model.HeroTextTop;
            HeroTextBottom = model.HeroTextBottom;
            HeroImage = $"{SilkSerumPageRepo.ImagePath}{model.HeroImage}";
            ParagraphUnderHeroFirst = model.ParagraphUnderHeroFirst;
            ParagraphUnderHeroSecond = model.ParagraphUnderHeroSecond;
            ParagraphUnderProducts = model.ParagraphUnderProducts;
            Link = model.Link;
            LinkName = model.LinkName;
            Advantage = model.Advantage;
            Email = model.Email;
            ContactUs = model.ContactUs;
            Manufacture = model.Manufacture;
            Street = model.Street;
            City = model.City;
            PostalCode = model.PostalCode;
            Copyright = model.Copyright;
            IsShowShadow = model.IsShowShadow;
        }
    }
}