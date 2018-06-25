using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ImagesShareingProject.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ImagesShareingProject.DAL
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("DefaultConnection")
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Profil> Profiles { get; set; }
        public DbSet<Rates> Rates { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Profil)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Rates>()
                .HasRequired(c => c.Profil)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
