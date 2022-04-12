using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nayelle.Models
{
    public class SilkSerumPage
    {
        public List<SilkSerum> Product { set; get; } = new List<SilkSerum>();
        public string Title { get; set; }
        public string HeroTextTop { get; set; }
        public string HeroTextBottom { get; set; }
        public string HeroImage { get; set; }
        public string ParagraphUnderHero { get; set; }
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
    }
}