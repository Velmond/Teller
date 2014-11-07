namespace Teller.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(1000)]
        [MinLength(2)]
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Published { get; set; }

        [Required]
        public bool IsFlagged { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual AppUser Author { get; set; }

        [ForeignKey("Story")]
        public int StoryId { get; set; }

        public virtual Story Story { get; set; }

        public virtual ICollection<CommentLike> Likes { get; set; }
    }
}
