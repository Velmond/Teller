namespace Teller.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Genre
    {
        public Genre()
        {
            this.Stories = new HashSet<Story>();
            this.Series = new HashSet<Series>();
        }

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(25)]
        [MinLength(2)]
        public string Name { get; set; }

        public virtual ICollection<Story> Stories { get; set; }

        public virtual ICollection<Series> Series { get; set; }
    }
}
