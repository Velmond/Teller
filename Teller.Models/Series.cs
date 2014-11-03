namespace Teller.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Series
    {
        public Series()
        {
            this.Stories = new HashSet<Story>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [MinLength(2)]
        [Required]
        public string Title { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual AppUser Author { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual ICollection<Story> Stories { get; set; }
    }
}
