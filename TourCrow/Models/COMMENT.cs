//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourCrow.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class COMMENT
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public string PlaceID { get; set; }
        public string Comment1 { get; set; }
    
        public virtual USER USER { get; set; }
    }
}
