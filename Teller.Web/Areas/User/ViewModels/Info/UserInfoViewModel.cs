﻿namespace Teller.Web.Areas.User.ViewModels.Info
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels.Base;

    public class UserInfoViewModel : UserViewModel
    {
        public static Expression<Func<AppUser, UserInfoViewModel>> FromUser
        {
            get
            {
                return user => new UserInfoViewModel()
                {
                    Username = user.UserName,
                    RegisteredOn = user.RegisteredOn,
                    Motto = user.UserInfo != null ? user.UserInfo.Motto : "not entered yet",
                    Description = user.UserInfo != null ? user.UserInfo.Description : "not entered yet",
                    AvatarPath = user.UserInfo != null ? user.UserInfo.AvatarPath : "/Images/UsersPictures/default/user.png",
                    StoriesCount = user.Stories.Any() ? user.Stories.Count() : 0,
                    StoryLikes = user.Stories.Any() ? user.Stories.Sum(s => s.Likes.Count(l => l.Value == true)) : 0,
                    StoryFavorites = user.Stories.Any() ? user.Stories.Sum(s => s.FavouritedBy.Count()) : 0,
                    Facebook = user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.Facebook : string.Empty) : string.Empty,
                    GooglePlus = user.UserInfo != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.GooglePlus : string.Empty) : string.Empty,
                    Twitter = user.UserInfo.Id != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.Twitter : string.Empty) : string.Empty,
                    YouTube = user.UserInfo.Id != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.YouTube : string.Empty) : string.Empty,
                    LinkedIn = user.UserInfo.Id != null ?
                                    (user.UserInfo.LinkedProfiles != null ?
                                       user.UserInfo.LinkedProfiles.LinkedIn : string.Empty) : string.Empty
                };
            }
        }

        public string Motto { get; set; }

        public string Description { get; set; }

        public DateTime RegisteredOn { get; set; }

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
