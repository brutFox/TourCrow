//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourCrow.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER
    {
        public USER()
        {
            this.COMMENTs = new HashSet<COMMENT>();
            this.PACKAGEs = new HashSet<PACKAGE>();
            this.RATINGs = new HashSet<RATING>();
        }
    
        public int UserID { get; set; }
        public string UserEmail { get; set; }
        public string UserFBID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    
        public virtual ICollection<COMMENT> COMMENTs { get; set; }
        public virtual ICollection<PACKAGE> PACKAGEs { get; set; }
        public virtual ICollection<RATING> RATINGs { get; set; }
    }
}
