using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourCrow.Models
{
    public class PlaceImageModel
    {
        public string status { get; set; }
        public ResultModel result { get; set; }
       
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











    }

   
}