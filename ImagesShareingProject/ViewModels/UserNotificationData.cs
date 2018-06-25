using ImagesShareingProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagesShareingProject.ViewModels
{
    public class UserNotificationData
    {
        public string User { get; set; }
        public int Action { get; set; }

        public int Rate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EventDate { get; set; }

        public string Img { get; set; }

        public int PostID { get; set; }
    }
}