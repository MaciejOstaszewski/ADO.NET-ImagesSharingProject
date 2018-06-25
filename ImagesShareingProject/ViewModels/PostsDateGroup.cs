using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagesShareingProject.ViewModels
{
    public class PostsDateGroup
    {

        [DataType(DataType.Date)]
        public DateTime? PostCreationDate { get; set; }
        public int PostCount { get; set; }
    }
}