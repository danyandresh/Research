using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DomeApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(DomeApp.Views.Resources), ErrorMessageResourceName = "postTitleRequired")]
        public string Title { get; set; }

        public string Content { get; set; }
    }
}