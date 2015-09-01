using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVCReports.Models;

namespace MVCReports.Context
{
    public class EntityContext : DbContext 
    {
        public virtual DbSet<AccuracyModel> Accuracy_Setup { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccuracyModel>().ToTable("Accuracy_Setup");
            base.OnModelCreating(modelBuilder);
        }

        public EntityContext()
            : base("name=DefaultConnection")
        {
        }
      
    }
}