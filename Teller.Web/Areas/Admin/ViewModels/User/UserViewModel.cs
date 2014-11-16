namespace Teller.Web.Areas.Admin.ViewModels.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    
    using Teller.Models;
    using Teller.Web.Infrastructure.Mapping;

    public class UserViewModel : IMapFrom<AppUser>
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Editable(false)]
        public string Username { get; set; }

        [Display(Name = "E-mail")]
        [Editable(false)]
        public string Email { get; set; }

        [Display(Name = "Registered")]
        [Editable(false)]
        public DateTime RegisteredOn { get; set; }

        [Display(Name = "Role")]
        public string RoleId { get; set; }

        [Display(Name = "Avatar")]
        public string AvatarPath { get; set; }

        [Display(Name = "Comment flags")]
        [Range(0, 100)]
        [Editable(false)]
        public byte? CommentFlags { get; set; }

        [Display(Name = "Story flags")]
        [Range(0, 100)]
        [Editable(false)]
        public byte? StoryFlags { get; set; }
    }
}
