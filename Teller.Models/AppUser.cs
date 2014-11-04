namespace Teller.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AppUser : IdentityUser
    {
        public AppUser()
            : base()
        {
            this.Stories = new HashSet<Story>();
            this.Favourites = new HashSet<Story>();
            this.ReadLater = new HashSet<Story>();
            this.Series = new HashSet<Series>();
            this.Likes = new HashSet<Like>();
            this.Comments = new HashSet<Comment>();
            this.Subscribers = new HashSet<AppUser>();
            this.SubscribedTo = new HashSet<AppUser>();
        }

        [MaxLength(100)]
        [MinLength(2)]
        public string Motto { get; set; }

        [MaxLength(500)]
        [MinLength(2)]
        public string Description { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public string AvatarPath { get; set; }

        [Required]
        public byte StoryViolations { get; set; }

        [Required]
        public int CommentViolations { get; set; }

        public virtual ICollection<Story> Stories { get; set; }

        public virtual ICollection<Story> Favourites { get; set; }
        
        public virtual ICollection<Story> ReadLater { get; set; }
        
        public virtual ICollection<Series> Series { get; set; }
        
        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<AppUser> Subscribers { get; set; }

        public virtual ICollection<AppUser> SubscribedTo { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
