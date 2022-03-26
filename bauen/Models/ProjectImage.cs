using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.Models
{
    public class ProjectImage:Base
    {
        public string Image { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
