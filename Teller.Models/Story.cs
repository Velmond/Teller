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
            this.Flags = new HashSet<Flag>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [MinLength(10)]
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DatePublished { get; set; }

        [Required]
        public string PicturePath { get; set; }

        [Required]
        public long ViewsCount { get; set; }

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

        public virtual ICollection<Flag> Flags { get; set; }
    }
}
