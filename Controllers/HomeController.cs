using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modells.Models;
namespace Modells.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Picture pic = new Picture();

            pic.pictureId = 1;
            pic.pictureTitle = "Route";
            pic.pictureDescription = "Une jolie route en automne";
            pic.pictureStandardSizeUrl = "../images/route.jpg";
            return View(pic);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Test(int? id)
        {
            int numberOfTotalPics = 100;
            int numberOfPicsToDisplayPerPage = 8;
            decimal n = numberOfTotalPics / numberOfPicsToDisplayPerPage;
            decimal numberOfPages = Math.Ceiling(n);
            ViewBag.totalPages = numberOfPages;
            ViewBag.Page = id;
            return View(id);
        }
        public ActionResult pictureDetails()
        {
            return View();
        }

    }
} 