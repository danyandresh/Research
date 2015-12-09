using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DomeApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(DomeApp.Views.Resources), ErrorMessageResourceName = "postTitleRequired")]
        public string Title { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(DomeApp.Views.Resources), ErrorMessageResourceName = "postExcerptRequired")]
        public string Excerpt { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(DomeApp.Views.Resources), ErrorMessageResourceName = "postContentRequired")]
        public string Content { get; set; }

        [Required]
        public virtual UserProfile Author { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}