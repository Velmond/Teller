namespace Teller.Web.Areas.User.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;

    public class UserInfoViewModel
    {
        public static Expression<Func<AppUser, UserInfoViewModel>> FromUser
        {
            get
            {
                return user => new UserInfoViewModel()
                {
                    Username = user.UserName,
                    AvatarPath = user.UserInfo != null ?
                                     user.UserInfo.AvatarPath :
                                     "/Images/UsersPictures/default/user.png",
                    StoriesCount = user.Stories.Count(),
                    StoryLikes = user.Stories.Sum(s => s.Likes.Count(l => l.Value == true)),
                    StoryFavorites = user.Stories.Sum(s => s.FavouritedBy.Count()),
                    Facebook = (user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.Facebook :
                                       string.Empty) :
                                    string.Empty),
                    GooglePlus = (user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.GooglePlus :
                                       string.Empty) :
                                    string.Empty),
                    Twitter = (user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.Twitter :
                                       string.Empty) :
                                    string.Empty),
                    YouTube = (user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.YouTube :
                                       string.Empty) :
                                    string.Empty),
                    LinkedIn = (user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.LinkedIn :
                                       string.Empty) :
                                    string.Empty)
                };
            }
        }

        public string Username { get; set; }

        public string AvatarPath { get; set; }

        public int StoriesCount { get; set; }

        public long StoryLikes { get; set; }

        public int StoryFavorites { get; set; }

        public string Facebook { get; set; }
        
        public string GooglePlus { get; set; }
        
        public string YouTube { get; set; }
        
        public string Twitter { get; set; }

        public string LinkedIn { get; set; }

    }
}
