using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesShareingProject.Models
{
    public class Comment
    {
        public Comment()
        {
        }

        public Comment(string contents, DateTime creationDate, int postID, int profilID, Comment reply)
        {
            Contents = contents;
            CreationDate = creationDate;
            PostID = postID;
            ProfilID = profilID;
            Reply = reply;
        }

        public int ID { get; set; }
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Comment cannot be longer than 250 characters.")]
        public string Contents { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public int PostID { get; set; }
        public virtual Post Post { get; set; }

        public int ProfilID { get; set; }
        public virtual Profil Profil { get; set; }

        public virtual Comment Reply { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }


    }
}


