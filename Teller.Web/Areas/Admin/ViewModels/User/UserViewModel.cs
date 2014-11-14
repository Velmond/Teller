namespace Teller.Web.Areas.Admin.ViewModels.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    
    using Teller.Models;
    using Teller.Web.Infrastructure.Mapping;

    public class UserViewModel : IMapFrom<AppUser>
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        public string Username { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Registered")]
        public DateTime RegisteredOn { get; set; }

        [Display(Name = "Role")]
        public string RoleId { get; set; }

        [Display(Name = "Avatar path")]
        public string AvatarPath { get; set; }

        [Display(Name = "Comment flags")]
        [Range(0, 100)]
        public int? CommentViolations { get; set; }

        [Display(Name = "Story flags")]
        [Range(0, 100)]
        public byte? StoryViolations { get; set; }
    }
}
