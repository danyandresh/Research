using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomeApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(DomeApp.Views.Resources), ErrorMessageResourceName = "postTitleRequired")]
        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }
    }
}