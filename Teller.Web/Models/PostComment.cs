namespace Teller.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostComment
    {
        [Required(ErrorMessage = "Message content is required to post a message... Duh o.O")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Message content must be between 2 and 1000 characters long")]
        public string CommentContent { get; set; }

        [Required]
        public int StoryId { get; set; }
    }
}