namespace Teller.Web.Areas.User.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;

    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels.Base;

    public class EditInfoViewModel : UserViewModel
    {
        public static Expression<Func<AppUser, EditInfoViewModel>> FromUser
        {
            get
            {
                return user => new EditInfoViewModel()
                {
                    Username = user.UserName,
                    AvatarPath = user.UserInfo.AvatarPath,
                    Picture = null,
                    Motto = user.UserInfo.Motto,
                    Description = user.UserInfo.Description,
                    Facebook = user.UserInfo.LinkedProfiles.Facebook,
                    GooglePlus = user.UserInfo.LinkedProfiles.GooglePlus,
                    LinkedIn = user.UserInfo.LinkedProfiles.LinkedIn,
                    Twitter = user.UserInfo.LinkedProfiles.Twitter,
                    YouTube = user.UserInfo.LinkedProfiles.YouTube
                };
            }
        }

        public HttpPostedFileBase Picture { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Motto must be between 2 and 100 characters long.")]
        public string Motto { get; set; }

        [Required]
        [DataType("TextArea")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 1000 characters long.")]
        [UIHint("TextArea")]
        public string Description { get; set; }

        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?facebook.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?", ErrorMessage = "Invalid Facebook profile url")]
        public string Facebook { get; set; }

        [DisplayName("Google+")]
        [RegularExpression(@"(?:(?:http|https):\/\/)?plus.google.com\/([+][A-Za-z0-9-_]+|[0-9]{21})(?:\/)?(?:[A-Za-z0-9-_]+)?$", ErrorMessage = "Invalid Google+ profile url")]
        public string GooglePlus { get; set; }

        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?linkedin.com(\w+:{0,1}\w*@)?(\S+)(:([0-9])+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?", ErrorMessage = "Invalid LinkedIn profile url")]
        public string LinkedIn { get; set; }

        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?twitter.com\/(#!\/)?[a-zA-Z0-9]{3,}", ErrorMessage = "Invalid Twitter profile url")]
        public string Twitter { get; set; }

        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?youtube.com\/(channel\/|user\/)[a-zA-Z0-9]{1,}", ErrorMessage = "Invalid YouTube profile url")]
        public string YouTube { get; set; }
    }
}