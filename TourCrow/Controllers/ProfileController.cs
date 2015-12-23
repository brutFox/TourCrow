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
        //public string username = null;
        //public string fbId = null;
        //public string packName = null;
        //public string packName;
        [HttpGet]
        public ActionResult Index()
        {
            string username = Convert.ToString(Request["userName"]);
            string fbId = Convert.ToString(Request["fbId"]);
            string packName;
            DateTime now = DateTime.Now;

            if (Request["packName"] == "")
            {
                packName = DateTime.Now.ToString();
            }
            else
            {
                packName = Convert.ToString(Request["packName"]);
            }

            if (username.IsNullOrWhiteSpace())
            {
                ViewBag.Title = username + "Profile";
            }
            else
            {
                ViewBag.Title = username + " | Profile";
            }

            ViewBag.username = username;
            ViewBag.fbID = fbId;

            showPackages(fbId);

            string email = Convert.ToString(Request["fbEmail"]);
            //Response.Write(email+"</br>");
            string[] pids = Request.QueryString.GetValues("pid");
            string[] photos = Request.QueryString.GetValues("photoid");
            //Response.Write(username + "</br>" + fbId + "</br>"+pids[0]);



            using (var context = new TourCrowDBEntities())
            {
                //var query = from usr in tcdb.USERs where usr.UserFBID == fbId select usr;
                //var chkusr = query.Count();
                //Response.Write(chkusr);
                try
                {
                    var query = from usr in context.USERs where usr.UserFBID == fbId select usr;
                    var chkusr = query.Count();
                    if (chkusr > 0 && pids.Length > 0)
                    {
                        insertPackage(pids, fbId, username, photos, packName, now);
                    }

                    else if (chkusr <= 0 && pids.Length > 0)
                    {
                        var user_insert_query = new USER { UserName = username, UserFBID = fbId };
                        context.USERs.Add(user_insert_query);
                        context.SaveChanges();

                        insertPackage(pids, fbId, username, photos, packName, now);
                    }
                }
                catch (Exception)
                {
                    //Response.Write("No Place Selected");
                }

            }

            return View();
        }

        public void showPackages(string fbId_new)
        {
            using (var packcon = new TourCrowDBEntities())
            {

                var pck_name = from pck in packcon.PACKAGEs where pck.UserFBID == fbId_new.ToString() select pck.Title;
                var check_pack_nm = pck_name.ToList();
                ViewBag.count_pack = pck_name.Count();

                ViewBag.packages = check_pack_nm;

                //var pck_date = from pck in packcon.PACKAGEs where pck.UserFBID == fbId_new.ToString() select pck.Date;
                //var check_pack = pck_date.ToList();
                //ViewBag.count_pack = pck_date.Count();
                
                //ViewBag.packages = check_pack;
            }

            //return ("Index");
        }

        //[HttpGet]
        //public ActionResult CheckSignIn()
        //{
        //    username = Convert.ToString(Request["userName"]);
        //    fbId = Convert.ToString(Request["fbId"]);
        //    string email = Convert.ToString(Request["fbEmail"]);
        //    //Response.Write(email+"</br>");
        //    string[] pids = Request.QueryString.GetValues("pid");
        //    string[] photos = Request.QueryString.GetValues("photoid");
        //    //Response.Write(username + "</br>" + fbId + "</br>"+pids[0]);
            
        //    using (tcdb)
        //    {
                
        //        var query = from usr in tcdb.USERs where usr.UserFBID == fbId.ToString() select usr;
        //        var chkusr = query.Count();
        //        //Response.Write(chkusr);
        //        try
        //        {
        //            if (chkusr > 0 && pids.Length > 0)
        //            {
        //                insertPackage(pids, fbId, username, photos);


        //            }

        //            else if (chkusr <= 0 && pids.Length > 0)
        //            {
        //                var user_insert_query = new USER { UserName = username, UserFBID = fbId };
        //                tcdb.USERs.Add(user_insert_query);
        //                tcdb.SaveChanges();

        //                insertPackage(pids, fbId, username, photos);
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            Response.Write("No Place Selected");
        //        }
                
        //    }

        //    return View("Index");
        //}

        private void insertPackage(string[] pids,string fbId,string username,string[] photos, string packName, DateTime now)
        {
            //insert package
            using (var dbcon = new TourCrowDBEntities())
            {
                
                var pack_insert_query = new PACKAGE {UserFBID = fbId, Date = now, Title = packName};
                dbcon.PACKAGEs.Add(pack_insert_query);
                dbcon.SaveChanges();
//( packName.ToString() == null ? Convert.ToString(DateTime.Now) : packName.ToString() )
                //get package id
                var get_packid = from pckid in dbcon.PACKAGEs
                    where pckid.UserFBID == fbId.ToString()
                    select pckid.PackageID;
                var packid = get_packid.AsEnumerable().LastOrDefault();

                for (int i = 0, j = 0; i < pids.Count() && j < photos.Count(); i++, j++)
                {
                    var userpack_insert_query = new USER_PACKAGE
                    {
                        PackageID = Convert.ToInt32(packid),
                        PlaceID = pids[i],
                        PhotoKey = photos[i]
                    };
                    dbcon.USER_PACKAGE.Add(userpack_insert_query);
                    dbcon.SaveChanges();
                }
            }
        }

    }
}
