using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nayelle.Models
{
    public class SilkSerumPage
    {
        public List<SilkSerum> Products { set; get; } = new List<SilkSerum>();
        public string Title { get; set; } = string.Empty;
        public string HeroTextTop { get; set; } = string.Empty;
        public string HeroTextBottom { get; set; } = string.Empty;
        public string HeroImage { get; set; } = string.Empty;
        public string ParagraphUnderHeroFirst { get; set; } = string.Empty;
        public string ParagraphUnderHeroSecond { get; set; } = string.Empty;
        public string ParagraphUnderProducts { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string LinkName { get; set; } = string.Empty;
        public string Advantage { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactUs { get; set; } = string.Empty;
        public string Manufacture { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public bool IsShowShadow { get; set; } = false;
    }

    public class SilkSerum
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ingredient { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
    }
}