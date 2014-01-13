namespace DomeApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class Comment
    {
        public int Id { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(DomeApp.Views.Resources), ErrorMessageResourceName = "commentContentRequired")]
        public string Content { get; set; }

        [Required]
        public virtual BlogPost Post { get; set; }

        [Required]
        public virtual UserProfile Author { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}