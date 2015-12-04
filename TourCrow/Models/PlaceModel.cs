using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourCrow.Models
{
    public class PlaceModel
    {
        public string status { get; set; }
        public List<ResultModel> results { get; set; }

        public class ResultModel
        {
            public string place_id { get; set; }
            public string reference { get; set; }
            public string name { get; set; }
            public string formatted_address { get; set; }
            public GeometryModel geometry;
            public List<PhotoModel> photos;

        }

        public class GeometryModel
        {
            public LocationModel location;
        }

        public class LocationModel
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class PhotoModel
        {
            public string photo_reference { get; set; }
        }
       
    }

    //public class PlaceDetailsModel
    //{
    //    public string status { get; set; }
    //    public List<PredictionModel> predictions { get; set; } 

    //    public class PredictionModel
    //    {
    //        public string description { get; set; }
    //        public string place_id { get; set; }
    //    }
    //}
    public class PlaceDetailsModel
    {
        public string status { get; set; }
        public List<ResultModel> results { get; set; }

        public class ResultModel
        {
            public string name { get; set; }
            public string formatted_address { get; set; }
            public string website { get; set; }
            public List<string> types { get; set; }
            public List<PhotoModel> photos { get; set; }

        }

        public class PhotoModel
        {
            public int height { get; set; }
            public int width { get; set; }
            public string photo_reference { get; set; }
        }

        //public class PlaceModel
        //{
        //    public string status { get; set; }
        //    public List<ResultModel> results { get; set; }

        //    public class ResultModel
        //    {
        //        public string id { get; set; }
        //        public string place_id { get; set; }
        //        public string reference { get; set; }
        //        public GeometryModel geometry;

        //    }

        //    public class GeometryModel
        //    {
        //        public LocationModel location;
        //    }

        //    public class LocationModel
        //    {
        //        public float lat { get; set; }
        //        public float lng { get; set; }
        //    }

        //}



    }
    

}