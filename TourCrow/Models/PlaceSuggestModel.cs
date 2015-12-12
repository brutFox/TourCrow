using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourCrow.Models
{
    public class PlaceSuggestModel
    {
        public string status { get; set; }
        public List<ResultModel> results { get; set; }

        public class ResultModel
        {
            public string place_id { get; set; }
            public string name { get; set; }
            public double rating { get; set; }
            //public string formatted_address { get; set; }
            public GeometryModel geometry;
            public List<PhotoModel> photos;
            public string vicinity { get; set; }

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
}