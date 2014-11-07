namespace Teller.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class Flag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public FlagType FlagType { get; set; }

        [Required]
        public bool IsResolved { get; set; }

        [Required]
        public DateTime DateFlagged { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        [ForeignKey("Story")]
        public int StoryId { get; set; }

        public virtual Story Story { get; set; }
    }
}
