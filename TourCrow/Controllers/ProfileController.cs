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

        [HttpGet]
        public ActionResult CheckSignIn()
        {
            string username = Convert.ToString(Request["userName"]);
            string fbId = Convert.ToString(Request["fbId"]);
            string email = Convert.ToString(Request["fbEmail"]);
            //Response.Write(email+"</br>");
            string[] pids = Request.QueryString.GetValues("pid");
            string[] photos = Request.QueryString.GetValues("photoid");
            //Response.Write(username + "</br>" + fbId + "</br>"+pids[0]);
            
            using (tcdb)
            {
                
                var query = from usr in tcdb.USERs where usr.UserFBID == fbId.ToString() select usr;
                var chkusr = query.Count();
                //Response.Write(chkusr);
                try
                {
                    if (chkusr > 0 && pids.Length > 0)
                    {
                        insertPackage(pids, fbId, username, photos);


                    }

                    else if (chkusr <= 0 && pids.Length > 0)
                    {
                        var user_insert_query = new USER { UserName = username, UserFBID = fbId };
                        tcdb.USERs.Add(user_insert_query);
                        tcdb.SaveChanges();

                        insertPackage(pids, fbId, username, photos);
                    }
                }
                catch (Exception)
                {

                    Response.Write("No Place Selected");
                }
                
            }

            return View("Index");
        }

        private void insertPackage(string[] pids,string fbId,string username,string[] photos)
        {
            //insert package
            var pack_insert_query = new PACKAGE { UserFBID = fbId };
            tcdb.PACKAGEs.Add(pack_insert_query);
            tcdb.SaveChanges();

            //get package id
            var get_packid = from pckid in tcdb.PACKAGEs where pckid.UserFBID == fbId.ToString() select pckid.PackageID;
            var packid = get_packid.AsEnumerable().LastOrDefault();

            //var items = pids.Zip(photos, (pid, photo) => new { pID = pid, phID = photo });
            //insert user-package
            foreach (string placeid in pids)
            {
                var userpack_insert_query = new USER_PACKAGE { PackageID = Convert.ToInt32(packid), PlaceID = placeid };
                //var userpack_insert_query = new USER_PACKAGE { PackageID = Convert.ToString(packid), PlaceID =  placeid.pID, PhotoKey=placeid};
                tcdb.USER_PACKAGE.Add(userpack_insert_query);
                tcdb.SaveChanges();
            }
        }

    }
}
