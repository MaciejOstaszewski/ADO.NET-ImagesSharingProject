using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagesShareingProject.Models
{
    public class Rates
    {
        public int ID { get; set; }


        public virtual Post Post { get; set; }
        public virtual Profil Profil { get; set; }

        public int Value { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

    }
}