using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesShareingProject.Models
{
    public class Post
    {
        public int ID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title cannot be longer than 30 characters.")]
        public string Title { get; set; }
        //[Required]
        [Display(Name = "Image URL address")]
        public string Image { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        //public int OpinionsP { get; set; }
        //public int OpinionsN { get; set; }

        public int ProfilID { get; set; }
        public virtual Profil Profile { get; set; }


        public virtual ICollection<Tag> Tags { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<Rates> Rates { get; set; }


    }
}
