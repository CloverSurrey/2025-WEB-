﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Music_Shopping.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Music_ShoppingEntities : DbContext
    {
        public Music_ShoppingEntities()
            : base("name=Music_ShoppingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<order_details> order_details { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<products_records> products_records { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
