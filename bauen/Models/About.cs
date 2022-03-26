using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.Models
{
    public class About: Base
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Buro { get; set; }
    }
}
