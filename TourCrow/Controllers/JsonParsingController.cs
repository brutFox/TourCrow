﻿using System;
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
    public class JsonParsingController : Controller
    {
        //
        // GET: /JsonParsing/

        public ActionResult Index()
        {

            string getHttpResponse = downloadWebPage(48.859294, 2.347589);
            //Response.Write(getHttpResponse);
            parseData(getHttpResponse);
            return View();

        }

        private void parseData(String json)
        {
            
            JavaScriptSerializer js = new JavaScriptSerializer();
            PlaceModel placeModel = js.Deserialize<PlaceModel>(json);

            List<PlaceModel.ResultModel> resultModel = placeModel.results;

            foreach (PlaceModel.ResultModel result in resultModel)
            {
                string getJsonData = place_details(result.place_id);
                //Response.Write(getJsonData);
                JavaScriptSerializer placeDetailsJS = new JavaScriptSerializer();
                PlaceDetailsModel placeImageModel = placeDetailsJS.Deserialize<PlaceDetailsModel>(getJsonData);
                //Response.Write(placeImageModel.result.name+"</br>");

                //break;
            }

            

        }
        
        //map API using latitude and longitude
        public static string downloadWebPage(double lng, double lat)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/radarsearch/json?location=" + lng + "," + lat + "&radius=500&types=tour&keyword=vegetarian&key=" + appKeys.GOOGLE_PLACE_API_KEY;

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

        public static string place_details(string place_id)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + place_id + "&key=" + appKeys.GOOGLE_PLACE_API_KEY;

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

        public static string place_autocomplete(string input)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + input + "&types=geocode&key=" + appKeys.GOOGLE_PLACE_API_KEY;

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
            string urlAddress = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference="+image_key+"&key="+appKeys.GOOGLE_PLACE_API_KEY;


            return urlAddress;
        }

        public static string Place_id(string place_id)
        {


            return "asd";
        }


    }



}
