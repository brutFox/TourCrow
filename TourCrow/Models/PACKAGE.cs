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
    
    public partial class PACKAGE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PACKAGE()
        {
            this.USER_PACKAGE = new HashSet<USER_PACKAGE>();
            this.IMAGEs = new HashSet<IMAGE>();
        }
    
        public int PackageID { get; set; }
        public string Title { get; set; }
        public string UserFBID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual USER USER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_PACKAGE> USER_PACKAGE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMAGE> IMAGEs { get; set; }
    }
}
