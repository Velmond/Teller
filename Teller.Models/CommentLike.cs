namespace Teller.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CommentLike
    {
        [Key, Column(Order = 0)]
        public string AuthorId { get; set; }

        [Key, Column(Order = 1)]
        public int CommentId { get; set; }

        public bool? Value { get; set; }

        public virtual AppUser Author { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
