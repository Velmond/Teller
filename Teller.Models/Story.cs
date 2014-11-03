namespace Teller.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Story
    {
        public Story()
        {
            this.Likes = new HashSet<Like>();
            this.Comments = new HashSet<Comment>();
            this.FavouritedBy = new HashSet<AppUser>();
            this.ToBeReadBy = new HashSet<AppUser>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [MinLength(2)]
        [Required]
        public string Title { get; set; }

        [MinLength(10)]
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DatePublished { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual AppUser Author { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        [ForeignKey("Series")]
        public int? SeriesId { get; set; }

        public virtual Series Series { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<AppUser> FavouritedBy { get; set; }

        public virtual ICollection<AppUser> ToBeReadBy { get; set; }
    }
}
