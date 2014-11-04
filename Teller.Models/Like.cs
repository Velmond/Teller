namespace Teller.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Like
    {
        [Key, Column(Order = 0)]
        public string AuthorId { get; set; }

        [Key, Column(Order = 1)]
        public string StoryId { get; set; }

        public bool? Value { get; set; }

        public virtual AppUser Author { get; set; }

        public virtual Story Story { get; set; }
    }
}
