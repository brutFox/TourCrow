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
            public string id { get; set; }
            public string place_id { get; set; }
            public string reference { get; set; }
            public GeometryModel geometry;

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
       
    }

    

}