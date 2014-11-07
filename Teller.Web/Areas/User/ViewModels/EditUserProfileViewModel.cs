namespace Teller.Web.Areas.User.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;

    public class EditUserProfileViewModel
    {
        public static Expression<Func<AppUser, EditUserProfileViewModel>> FromUser
        {
            get
            {
                return user => new EditUserProfileViewModel()
                {
                    Username = user.UserName,
                    AvatarPath = user.UserInfo.AvatarPath,
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

        public string Username { get; set; }

        public string AvatarPath { get; set; }

        public string Motto { get; set; }

        public string Description { get; set; }

        public string Facebook { get; set; }

        public string GooglePlus { get; set; }

        public string LinkedIn { get; set; }

        public string Twitter { get; set; }

        public string YouTube { get; set; }
    }
}