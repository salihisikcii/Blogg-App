using System.ComponentModel.DataAnnotations;
using BlogApps.Entity;

namespace BlogApps.Models

{
    public class NewPostsViewModel
     {
         [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }
        
        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required]
        
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

         [Display(Name = "PostId")]
        public int PostId { get; set; }
     
         public bool IsActive { get; set; }

         public List<Tag> Tags { get; set; } = new();


     }
}
