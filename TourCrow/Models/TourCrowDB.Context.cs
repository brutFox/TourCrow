﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TourCrowDBEntities : DbContext
    {
        public TourCrowDBEntities()
            : base("name=TourCrowDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<COMMENT> COMMENTs { get; set; }
        public virtual DbSet<PACKAGE> PACKAGEs { get; set; }
        public virtual DbSet<RATING> RATINGs { get; set; }
        public virtual DbSet<USER> USERs { get; set; }
        public virtual DbSet<USER_PACKAGE> USER_PACKAGE { get; set; }
        public virtual DbSet<IMAGE> IMAGEs { get; set; }
    }
}
