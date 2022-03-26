using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bauen.Models
{
    public class Banner: Base
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
