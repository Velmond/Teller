namespace Teller.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserInfo
    {
        public int Id { get; set; }

        [ForeignKey("LinkedProfiles")]
        public int LinkedProfilesId { get; set; }

        public virtual LinkedProfiles LinkedProfiles { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string Motto { get; set; }

        [StringLength(1000, MinimumLength = 2)]
        public string Description { get; set; }

        public string AvatarPath { get; set; }

        [Required]
        public byte StoryViolations { get; set; }

        [Required]
        public int CommentViolations { get; set; }
    }
}
