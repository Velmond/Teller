namespace Teller.Web.Areas.User.ViewModels.Base
{
    using System.ComponentModel.DataAnnotations;

    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string AvatarPath { get; set; }
    }
}