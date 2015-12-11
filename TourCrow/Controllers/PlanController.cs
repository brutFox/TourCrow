using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using TourCrow.App_Start;
using TourCrow.Models;

namespace TourCrow.Controllers
{
    public class PlanController : Controller
    {
        //
        // GET: /Plan/

        public ActionResult Index()
        {
            //string getHttpResponse = place_details("Ratargul");
            ////Response.Write(getHttpResponse);
            //parseData(getHttpResponse);

            return View();
        }

        [HttpPost]
        public ActionResult Search_Place()
        {
            string input = Convert.ToString(Request["Searchbar"]);

            string getHttpResponse = place_details(input);

            parseData(getHttpResponse);

            ViewBag.place_inpur = input;

            return View("Index");
        }


        public void parseData(String json)
        {
            //Response.Write(json);

            JavaScriptSerializer js = new JavaScriptSerializer();
            PlaceModel placeModel = js.Deserialize<PlaceModel>(json);

            /*PlaceModel.ResultModel resultModel = placeModel.results;*/

            foreach (PlaceModel.ResultModel result in placeModel.results)
            {
                //Response.Write(result.place_id + "</br>" + result.geometry.location.lat);
                ViewBag.pm = placeModel.results;
                ViewBag.place_id = result.place_id;
                ViewBag.place_name = result.name;
                ViewBag.place_lat = result.geometry.location.lat;
                ViewBag.place_lng = result.geometry.location.lng;

                string jsonData = place_suggest(result.geometry.location.lat, result.geometry.location.lng);
                parseDataToShowSugg(jsonData);


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
        }

                //https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=23.8105444,90.3272611&radius=5000&types=hospital&key=AIzaSyAX1EHCUo6oibCxw3gKDuot3r6B-2wrm2s


        public static string place_details(string input)
        {
            //string urlAddress = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + place_id + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;
            string urlAddress = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + input + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;
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

        private void parseDataToShowSugg(String json)
        {
            //Response.Write(json);

            JavaScriptSerializer js = new JavaScriptSerializer();
            PlaceSuggestModel placeSuggModel = js.Deserialize<PlaceSuggestModel>(json);

            /*PlaceModel.ResultModel resultModel = placeModel.results;*/

            foreach (PlaceSuggestModel.ResultModel result in placeSuggModel.results)
            {
                Response.Write(result.name + "</br>" + result.geometry.location.lat);
                //ViewBag.place_id = result.place_id;
                //ViewBag.place_name = result.name;
                //ViewBag.place_lat = result.geometry.location.lat;
                //ViewBag.place_lng = result.geometry.location.lng;
                try
                {
                    //Response.Write(result.photos[0].photo_reference);
                    ViewBag.place_photo = result.photos[0].photo_reference;
                    //func

                }
                catch (Exception e)
                {
                    //Response.Write(e.Message);

                }

            }
        }



        public static string place_suggest(double lat, double lng)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + lat + "," + lng + "&radius=5000&types=hospital&key=" + appKeys.GOOGLE_PLACE_API_KEY;
           
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

        public static string place_image(string image_key)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + image_key + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;


            return urlAddress;
        }
    }
}
