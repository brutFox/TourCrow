using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TourCrow.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            ViewBag.activePage = "Home";
            return View();
        }

        public ActionResult AboutUs()
        {
            ViewBag.Title = "About Us";
            ViewBag.activePage = "About";
            return View();
        }
    }
}
