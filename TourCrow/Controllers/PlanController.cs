﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using TourCrow.App_Start;
using TourCrow.Models;


namespace TourCrow.Controllers
{
    public class PlanController : Controller
    {
        //
        // GET: /Plan/

        //private TourCrowDBEntities tcdb = new TourCrowDBEntities();
        public string search_val = "null";
        public int packId;
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Plan";
            ViewBag.activePage = "Plan";
            string username = Convert.ToString(Request["userName"]);
            string fbId = Convert.ToString(Request["fbId"]);
            string packName;
            packId = Convert.ToInt32(Request["packid"]);
            ViewBag.packID = packId;
            DateTime now = DateTime.Now;

            ViewBag.name = username;
            ViewBag.id = fbId;
            search_val = Convert.ToString(Request["search"]) == null ? "null" : Convert.ToString(Request["search"]);

            ViewBag.pageGetValue = search_val;

            string email = Convert.ToString(Request["fbEmail"]);
            //Response.Write(email+"</br>");
            string[] pids = Request.QueryString.GetValues("pid");
            string[] photos = Request.QueryString.GetValues("photoid");
            if (Request["packName"] == "")
            {
                packName = DateTime.Now.ToString();
            }
            else
            {
                packName = Convert.ToString(Request["packName"]);
            }

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

        private void insertPackage(string[] pids, string fbId, string username, string[] photos, string packName, DateTime now)
        {
            //insert package
            using (var dbcon = new TourCrowDBEntities())
            {

                var pack_insert_query = new PACKAGE { UserFBID = fbId, Date = now, Title = packName };
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

        [HttpPost]
        public ActionResult Search_Place()
        {
            string input = Convert.ToString(Request["Searchbar"]);

            string getHttpResponse = place_details(input);

            parseData(getHttpResponse);

            ViewBag.place_inpur = input;
            Response.Write(ViewBag.nm);

            return View("Index");
        }


        [HttpGet]
        public ActionResult getJsonData()
        {
            string data = Convert.ToString(Request["place_name"]);
            string getHttpResponse = place_details(data);
            PlaceSuggestModel placeSuggestModel = parseData(getHttpResponse);
            string getPlaceJson = "";
            getPlaceJson += "[";
            int totalDataSize = placeSuggestModel == null || placeSuggestModel.results == null ? 0 : placeSuggestModel.results.Count;
            for (int x = 0; x < totalDataSize; x++)
            {
                getPlaceJson += " { \"place_id\" : " + "\"" + placeSuggestModel.results[x].place_id + "\",";
                getPlaceJson += " \"place_name\" : " + "\"" + placeSuggestModel.results[x].name + "\",";
                getPlaceJson += " \"place_address\" : " + "\"" + placeSuggestModel.results[x].vicinity + "\",";

                if (placeSuggestModel.results[x].photos != null)
                    getPlaceJson += " \"place_photo\" : " + "\"" + placeSuggestModel.results[x].photos[0].photo_reference + "\",";
                else
                    getPlaceJson += " \"place_photo\" : " + "\"\" ,";

                getPlaceJson += " \"place_longitude\" : " + "\"" + placeSuggestModel.results[x].geometry.location.lng + "\",";
                getPlaceJson += " \"place_latitude\" : " + "\"" + placeSuggestModel.results[x].geometry.location.lat + "\",";

                if (x == (totalDataSize - 1))
                    getPlaceJson += " \"place_rating\" : " + "\"" + placeSuggestModel.results[x].rating + "\"}";
                else
                    getPlaceJson += " \"place_ratting\" : " + "\"" + placeSuggestModel.results[x].rating + "\"},";
            }
            getPlaceJson += "]";
            ViewBag.sendData = getPlaceJson;
            return View("json");
        }


        public PlaceSuggestModel parseData(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            PlaceModel placeModel = js.Deserialize<PlaceModel>(json);
            PlaceSuggestModel getPlaceSuggestModel = null;


            foreach (PlaceModel.ResultModel result in placeModel.results)
            {
                string jsonData = place_suggest(result.geometry.location.lat, result.geometry.location.lng);
                getPlaceSuggestModel = parseDataToShowSugg(jsonData);
                break;


                try
                {
                    //Response.Write(result.photos[0].photo_reference);
                    ViewBag.place_photo = result.photos[0].photo_reference;
                    //func

                }
                catch (Exception e)
                {
                    Response.Write(e.Message);

                }
            }
            return getPlaceSuggestModel;
        }

                //https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=23.8105444,90.3272611&radius=5000&types=hospital&key=AIzaSyAX1EHCUo6oibCxw3gKDuot3r6B-2wrm2s


        public static string place_details(string input)
        {
     
            string urlAddress = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + input + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if(response.CharacterSet == null)
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

        private PlaceSuggestModel parseDataToShowSugg(String json)
        {
            //Response.Write(json);

            JavaScriptSerializer js = new JavaScriptSerializer();
            PlaceSuggestModel placeSuggModel = js.Deserialize<PlaceSuggestModel>(json);

            return placeSuggModel;

            /*PlaceModel.ResultModel resultModel = placeModel.results;*/
            List<PlaceSuggestModel.ResultModel> n = new List<PlaceSuggestModel.ResultModel>(); 
                
            foreach (PlaceSuggestModel.ResultModel result in placeSuggModel.results)
            {
                //Response.Write(result.name + "</br>");
                //List<PlaceSuggestModel> n = new List<PlaceSuggestModel>(); 
                if (result != null)
                    n.Add(result);
                
                //ViewBag.na = n;
                
                
                try
                {
                    //Response.Write(result.photos[0].photo_reference);
                    //ViewBag.place_photo = result.photos[0].photo_reference;
                    //func

                }
                catch (Exception e)
                {
                    //Response.Write(e.Message);

                }

            }


            foreach (PlaceSuggestModel.ResultModel r in n)
                Response.Write(r.name);

            return null;
        }



        public static string place_suggest(double lat, double lng)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + lat + "," + lng + "&radius=50000&types=natural_feature|lodging|amusement_park|cafe|food|park|mosque|cemetery|church|movie_theater|hindu_temple|zoo|stadium|library|shopping_mall|restaurant|point_of_interest&key=" + appKeys.GOOGLE_PLACE_API_KEY;
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if(response.CharacterSet == null)
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

        public static string place_image(string image_key)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + image_key + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;


            return urlAddress;
        }
        [HttpGet]
        public ActionResult place_submit()
        {
            string[] items = Request.QueryString.GetValues("pid");

            foreach (string values in items)
            {
                
            }


            return View("");
        }
        public double[] latway;
        public double[] lngway;
        public string showMap(double latorigin, double lngorigin, double[] latdest, double[] lngdest)
        {
            
            string mapURL = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyAX1EHCUo6oibCxw3gKDuot3r6B-2wrm2s&origin="+latorigin+","+lngorigin+"&destination="+latdest[latdest.Length-1]+","+lngdest[lngdest.Length-1]+"&avoid=tolls|highways&waypoints=";

            return mapURL;
        }

       
    }
}
