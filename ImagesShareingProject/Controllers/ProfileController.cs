using ImagesShareingProject.DAL;
using ImagesShareingProject.Models;
using ImagesShareingProject.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImagesShareingProject.Controllers
{
    public class ProfileController : Controller
    {
        private ProjectContext db = new ProjectContext();
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Settings()
        {
            Profil profil = db.Profiles.SingleOrDefault(p => p.UserName == User.Identity.Name );
            ViewBag.Profil = profil;
            return View();
        }
        [Authorize]
        public ActionResult EditProfile(string nick)
        {
            Profil profil = db.Profiles.SingleOrDefault(p => p.UserName == User.Identity.Name);
            profil.Nick = nick;
            string avatar = "cucumber.jpg";
            HttpPostedFileBase file = Request.Files["image"];

            if (file != null && file.ContentLength > 0)
            {
                avatar = file.FileName;
                file.SaveAs(HttpContext.Server.MapPath("~/Images/") + avatar);
            }

            profil.Avatar = avatar;
            db.Entry(profil).State = EntityState.Modified;
            db.SaveChangesAsync();
            return RedirectToAction("Profile", "Posts", new { profileID = profil.ID });
        }

        [Authorize]
        public ActionResult Notifications(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            Profil profil = db.Profiles.SingleOrDefault(p => p.UserName == User.Identity.Name);
            //IQueryable<UserNotificationData> data = from post in db.Posts
            //                                        where post.ProfilID == profil.ID
            //                                        select new UserNotificationData()
            //                                        {
            //                                            Post = post,

            //                                  };
            List<UserNotificationData> data = new List<UserNotificationData>();
            List<Post> posts = db.Posts.Where(p => p.Profile.ID == profil.ID).ToList();
            foreach (Post p in posts)
            {
                foreach(Comment c in p.Comments)
                {
                    data.Add(
                        new UserNotificationData
                        {
                            Img = p.Image,
                            Action = 1,
                            EventDate = c.CreationDate,
                            Rate = 0,
                            User = c.Profil.Nick,
                            PostID = p.ID                          
                        });
                }
                foreach (Rates r in p.Rates)
                {
                    data.Add(
                        new UserNotificationData
                        {
                            Img = p.Image,
                            Action = 2,
                            EventDate = r.CreationDate,
                            Rate = r.Value,
                            User = r.Profil.Nick,
                            PostID = p.ID
                        });
                }

            }

            return View(data.OrderBy(d => d.EventDate).ToPagedList(pageNumber, pageSize));


        }
    }
}