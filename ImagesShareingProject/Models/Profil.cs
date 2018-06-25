using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesShareingProject.Models
{
    public class Profil
    {
        public int ID { get; set; }
        public string UserName { get; set; }

        public string Nick { get; set; }

        public string Avatar { get; set; }

        public virtual ICollection<Rates> Rates { get; set; }
    }
}
