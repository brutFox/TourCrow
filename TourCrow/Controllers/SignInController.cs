using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourCrow.Models;

namespace TourCrow.Controllers
{
    public class SignInController : Controller
    {
        //
        // GET: /SignIn/

        public ActionResult Index()
        {
            using (var context = new TourCrowDBEntities())
            {

                //var user = new USER { UserName = "klk sd", UserFBID = "123456", UserEmail = "klk@gmai.com" };
                //context.USERs.Add(user);
                //context.SaveChanges();


                //var qr = from c in context.USERs select c.UserEmail;
                ////var qr = context.
                //var item = qr.Count();
                //Response.Write(item);
            }

            return View();
        }

    }
}
