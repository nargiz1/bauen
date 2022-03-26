using bauen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.HomeViewModels
{
    public class HomeViewModel
    {
        public List<Banner> Banners { get; set; }
        public About AboutUs { get; set; }
        public List<Project> Projects { get; set; }

    }
}
