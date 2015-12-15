using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TourCrow.Models;

namespace TourCrow.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        TourCrowDBEntities tcdb = new TourCrowDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        //var user = new USER { UserName = "klk sd", UserFBID = "123456", UserEmail = "klk@gmai.com" };
        //context.USERs.Add(user);
        //context.SaveChanges();


        //var qr = from c in context.USERs select c.UserEmail;
        //var item = qr.Count();
        //Response.Write(item);


        [HttpGet]
        public ActionResult CheckSignIn()
        {
            string username = Convert.ToString(Request["userName"]);
            string fbId = Convert.ToString(Request["fbId"]);
            string[] items = Request.QueryString.GetValues("pid");
            //Response.Write(username + "</br>" + fbId + "</br>"+items[0]);
            
            using (tcdb)
            {
                var query = from usr in tcdb.USERs where usr.UserFBID == fbId.ToString() select usr;
                var chkusr = query.Count();
                //Response.Write(chkusr);
                if (chkusr > 0 && items.Length > 0 )
                {
                    var pack_insert_query = new PACKAGE {UserFBID = fbId};
                    tcdb.PACKAGEs.Add(pack_insert_query);
                    tcdb.SaveChanges();

                    var get_packid = from pckid in tcdb.PACKAGEs where pckid.UserFBID == fbId.ToString() select pckid.PackageID;
                    var packid = get_packid.FirstOrDefault();

                    foreach (string placeid in items)
                    {
                        var userpack_insert_query = new USER_PACKAGE { PackageID = packid, PlaceID = placeid };
                        tcdb.USER_PACKAGE.Add(userpack_insert_query);
                        tcdb.SaveChanges();
                    }
                    
                }

            }

            return View("Index");
        }

    }
}
