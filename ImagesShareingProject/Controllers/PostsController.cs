using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImagesShareingProject.DAL;
using ImagesShareingProject.Models;
using PagedList;
using System.Configuration;
using ImagesShareingProject.ViewModels;

namespace ImagesShareingProject.Controllers
{
    //TODO
    // Username in headaer
    //  Emails
    // Validations
    // Roles in new users
    // Routing
    public class PostsController : Controller
    {
        private ProjectContext db = new ProjectContext();
        [ActionName("Index")]
        public ViewResult Index(int? id, int? filter, int? page, string searchFilter, bool? active)
        {

            bool status = (active ?? true);
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            if (filter == 3)
            {
                return View(db.Posts.Where(p => p.Active == status && p.Title.Contains(searchFilter))
                    .Include(p => p.Category)
                    .Include(p => p.Profile)
                    .OrderByDescending(p => p.CreationDate)
                    .ToPagedList(pageNumber, pageSize));
            }
            if (filter == 1)
            {
                return View(db.Posts.Where(p => p.Active == status)
                    .Include(p => p.Category)
                    .Include(p => p.Profile)
                    .Where(p => p.CategoryID == id).OrderByDescending(p => p.CreationDate)
                    .ToPagedList(pageNumber, pageSize));
            }


            if (filter == 2)
            {
                return View(db.Posts.Where(p => p.Active == status)
                    .Include(p => p.Category)
                    .Include(p => p.Profile)
                    .Where(p => p.Tags.FirstOrDefault(t => t.ID == id).ID == id).OrderByDescending(p => p.CreationDate)
                    .ToPagedList(pageNumber, pageSize));
            }

            if (filter == 4)
            {
                return View(db.Posts.Where(p => p.Active == true)
                    .Include(p => p.Category)
                    .Include(p => p.Profile)
                    .Where(p => p.Profile.UserName == searchFilter)
                    .OrderByDescending(p => p.CreationDate)
                    .ToPagedList(pageNumber, pageSize));
            }

            return View(db.Posts
                .Where(p => p.Active == status)
                .Include(p => p.Category)
                .Include(p => p.Profile).OrderByDescending(p => p.CreationDate)
                .ToPagedList(pageNumber, pageSize));
        }

        // GET: Posts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }



            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ProfilID = new SelectList(db.Profiles, "ID", "UserName");
            ViewData["tags"] = db.Tags.ToList();


            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post, int[] tags, int imgType, string image)
        {
            ////  if (ModelState.IsValid)
            //   {

            if ((image == null || image == "") && imgType == 1)
            {
                return RedirectToAction("Create", new { message = "Select proper image" });
            }

            if (imgType == 1)
            {
                var myUniqueFileName = $@"{Guid.NewGuid()}.jpg";
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new System.Uri(image), HttpContext.Server.MapPath("~/Images/") + myUniqueFileName);
                }

                post.Image = myUniqueFileName;
            }
            else if (imgType == 2)
            {
                HttpPostedFileBase file = Request.Files["image"];
                if (file.FileName == "")
                {
                    return RedirectToAction("Create", new { message = "Select proper image" });
                }
                if (file != null && file.ContentLength > 0)
                {
                    post.Image = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + post.Image);
                }
            }

            post.CreationDate = DateTime.UtcNow.Date;

            post.ProfilID = db.Profiles.Single(p => p.UserName == User.Identity.Name).ID;
            post.Active = false;
            post.Tags = new List<Tag>();
            foreach (var item in tags)
            {
                post.Tags.Add(db.Tags.Single(t => t.ID == item));
            }
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            //  }

            //ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", post.CategoryID);
            //ViewBag.ProfilID = new SelectList(db.Profiles, "ID", "UserName", post.ProfilID);
            //ViewData["tags"] = db.Tags.ToList();
            //return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", post.CategoryID);
            ViewBag.ProfilID = new SelectList(db.Profiles, "ID", "UserName", post.ProfilID);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", post.CategoryID);
            ViewBag.ProfilID = new SelectList(db.Profiles, "ID", "UserName", post.ProfilID);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<ActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            if (post == null)
            {
                return HttpNotFound();
            }
            return Redirect(returnUrl);
            //return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> ActivatePost(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            post.Active = true;
            db.Entry(post).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> Rate(int id, int value, string returnUrl)
        {
            Post post = await db.Posts.FindAsync(id);
            if (post.Rates.Where(r => r.Profil.UserName == User.Identity.Name).ToList().Count != 0)
            {
                return RedirectToAction("Index");
            }
            post.Rates.Add(new Rates
            {
                Post = post,
                Profil = db.Profiles.FirstOrDefault(p => p.UserName == User.Identity.Name),
                Value = value,
                CreationDate = DateTime.UtcNow.Date

            });
            db.Entry(post).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Redirect(returnUrl);
            // return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddComment(int idp, string comment, string reply)
        {


            Post post = await db.Posts.FindAsync(idp);
            Comment comm;
            if (reply == null || reply == "")
            {
                comm = new Comment(comment, DateTime.UtcNow.Date, idp,
                db.Profiles.Single(p => p.UserName == User.Identity.Name).ID,
                null);
                
            } else
            {
                int replyID = Int32.Parse(reply);
                comm = new Comment(comment, DateTime.UtcNow.Date, idp,
                db.Profiles.Single(p => p.UserName == User.Identity.Name).ID,
                db.Comments.SingleOrDefault(c => c.ID == replyID));
            }

            post.Comments.Add(comm);
            db.Entry(post).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = idp });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendMailMessage(string to, string subject, string body)
        {
            SendMail(to, subject, body);
            return RedirectToAction("Index", new { message = "Sending email" });
        }

        public ViewResult MailForm()
        {
            return View();
        }

        public ActionResult Stats()
        {
            IQueryable<PostsDateGroup> data = from post in db.Posts
                                              group post by post.CreationDate into dateGroup
                                              select new PostsDateGroup()
                                              {
                                                  PostCreationDate = dateGroup.Key,
                                                  PostCount = dateGroup.Count()
                                              };
            return View(data.ToList());
        }

        public new ActionResult Profile(int? profileID)
        {
            ProfliData pd = new ProfliData
            {
                PostCount = db.Posts.Where(p => p.ProfilID == profileID).ToList().Count(),
                CommentsCount = db.Comments.Where(c => c.ProfilID == profileID).ToList().Count(),
                RateCount = db.Rates.Where(c => c.Profil.ID == profileID).ToList().Count()
            };
            Profil profil = db.Profiles.SingleOrDefault(p => p.ID == profileID);
            ViewBag.Profil = profil;
            return View(pd);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public static void SendMail(string to, string subject, string body)
        {
            var message = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["sender"], to)
            {
                Subject = subject,
                Body = body
            };

            var smtpClient = new System.Net.Mail.SmtpClient
            {
                Host = ConfigurationManager.AppSettings["smtpHost"],
                Credentials = new System.Net.NetworkCredential(
                    ConfigurationManager.AppSettings["sender"],
                    ConfigurationManager.AppSettings["passwd"]),
                EnableSsl = true
            };
            smtpClient.Send(message);


        }

        public String GetUser() => db.Profiles.Single(p => p.UserName == User.Identity.Name).Nick;
        public int GetUserID() => db.Profiles.Single(p => p.UserName == User.Identity.Name).ID;
        //public String GetUserAvatar() => db.Profiles.Single(p => p.UserName == User.Identity.Name).Avatar;
    }
}
