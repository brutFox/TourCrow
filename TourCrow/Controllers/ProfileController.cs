using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using TourCrow.App_Start;
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
            int packId= Convert.ToInt32(Request["packid"]);
        
            DateTime now = DateTime.Now;

            ViewBag.name = username;
            ViewBag.id = fbId;

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

            showPackages(fbId);
            getPackageInfo(fbId,packId);

            return View();
        }

        private void getPackageInfo(String fbId, int packId)
        {
            using (var pack = new TourCrowDBEntities())
            {
                var pck_id = from p in pack.USER_PACKAGE where p.PackageID == packId select p.PlaceID;
                var pck_count = pck_id.ToList();
                ViewBag.pck_count = pck_id.Count();
                List<string> pname = new List<string>();
                List<string> padd = new List<string>();
                List<string> pimg = new List<string>();
                int i = 0;
                foreach (var placeId in pck_count)
                {
                    String json = place_details(placeId);
                    ProfilePageModel.RootObject model = parseData(json);
                    pname.Add(model.result.name);
                    padd.Add(model.result.formatted_address);
                    pimg.Add(model.result.photos[0].photo_reference);
                    i++;
                }
                ViewBag.pname = pname;
                ViewBag.padd = padd;
                ViewBag.pimg = pimg;
            }
        }

        public void showPackages(string fbId_new)
        {
            using (var packcon = new TourCrowDBEntities())
            {

                var pck_name = from pck in packcon.PACKAGEs where pck.UserFBID == fbId_new.ToString() select pck.Title;
                
                var check_pack_nm = pck_name.ToList();
                
                ViewBag.count_pack = pck_name.Count();
                //Response.Write(pck_name.Count());
                ViewBag.packages = check_pack_nm;
            
                var pck_id = from pid in packcon.PACKAGEs where pid.UserFBID == fbId_new select pid.PackageID;
                var check_pid = pck_id.ToList();
                ViewBag.pid = check_pid;


            }
            //return ("Index");
        }

        public string place_details(string placeId)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + placeId + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return data;
            }
            return null;
        }

        public ProfilePageModel.RootObject parseData(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ProfilePageModel.RootObject model = js.Deserialize<ProfilePageModel.RootObject>(json);
            return model;
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

        public ActionResult fileup()
        {
            return View("fileup");
        }

        [HttpPost]
        public ActionResult fileupload(IEnumerable<HttpPostedFileBase> file)
        {

            foreach (var f in file)
            {
                if (f != null && f.ContentLength > 0 )
                {
                    string extension = Path.GetExtension(f.FileName);
                    var fileName = Path.GetFileName(f.FileName);
                    //var fileName = f + extension;
                    var path = Path.Combine(Server.MapPath("~/Content/Album"), fileName);
                    f.SaveAs(path);

                }
                else
                {
                    ViewBag.PropicError = "Invalid Photo";
                }
            }

            //return RedirectToAction("Index", HttpContext.User.Identity.Name.Split('|')[1].ToString());
            return View("fileup");
        }

    }
}
