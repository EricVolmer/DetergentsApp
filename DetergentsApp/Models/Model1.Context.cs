﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DetergentsApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DetergentsEntities : DbContext
    {
        public DetergentsEntities()
            : base("name=DetergentsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SheetType> SheetTypes { get; set; }
        public virtual DbSet<UserFile> UserFiles { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<vendorLogin> vendorLogin { get; set; }
    }
}
