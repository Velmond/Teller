namespace Teller.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostComment
    {
        [MaxLength(1000)]
        [MinLength(2)]
        [Required]
        public string CommentContent { get; set; }

        [Required]
        public int StoryId { get; set; }
    }
}