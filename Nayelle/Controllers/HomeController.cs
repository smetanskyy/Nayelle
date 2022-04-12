using Nayelle.Data.Repositories;
using Nayelle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nayelle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var repo = new SilkSerumPageRepo();
            var model = new SilkSerumVM(repo.SilkSerumPage);
            return View(model);
        }
    }
}