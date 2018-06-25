using ImagesShareingProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImagesShareingProject.DAL
{
    public class ProjectInitializer : DropCreateDatabaseIfModelChanges<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));

            var user = new ApplicationUser { UserName = "test@test2.com" };
            string passwd = "Administrator.11";

            var user2 = new ApplicationUser { UserName = "test@test3.com" };
            string passwd2 = "Administrator.11";

            userManager.Create(user2, passwd2);
            userManager.AddToRole(user2.Id, "User");

            userManager.Create(user, passwd);
            userManager.AddToRole(user.Id, "Admin");

            var profile = new List<Profil>
            {
                new Profil { Nick = "Admin", UserName = user.UserName, Avatar = "cucumber.jpg" },
                new Profil { Nick = "User", UserName = user2.UserName, Avatar = "cucumber.jpg" }
            };
            profile.ForEach(p => context.Profiles.Add(p));
            context.SaveChanges();


            var tags = new List<Tag>
            {
                new Tag { Name = "Obrazek" },
                new Tag { Name = "Humor" },
                new Tag { Name = "Cieakwostki" }
            };

            tags.ForEach(p => context.Tags.Add(p));
            context.SaveChanges();

            var category = new List<Category>
            {
                new Category { Name = "Obrazki" },
                new Category { Name = "Komiksy" },
                new Category { Name = "Memy" },
                new Category { Name = "Galeria" }
            };

            category.ForEach(p => context.Categories.Add(p));
            context.SaveChanges();




            var posts = new List<Post>
            {
                new Post { Title = "Obrazek", Image = "17688ce9-01af-42cb-b3d0-5c2e1384e186.jpg", CreationDate = DateTime.Parse("2018-09-02"), Active = true,
                           Profile = profile[0],
                           Category = category[0],
                           Tags = new List<Tag> { tags[0], tags[1] }
                },
                new Post { Title = "Obrazek", Image = "f207e44f-d59c-460b-8bb8-b8dfed49e75c.jpg", CreationDate = DateTime.Parse("2018-09-02"), Active = false,
                           Profile = profile[0],
                           Category = category[1],
                           Tags = new List<Tag> { tags[0], tags[1], tags[2] }
                }
            };


            posts.ForEach(p => context.Posts.Add(p));
            context.SaveChanges();

        }
    }
}