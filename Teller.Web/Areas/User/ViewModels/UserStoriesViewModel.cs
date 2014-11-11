namespace Teller.Web.Areas.User.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels.Base;
    using Teller.Web.ViewModels;

    public class UserStoriesViewModel : UserViewModel
    {
        public static Expression<Func<AppUser, UserStoriesViewModel>> FromUser
        {
            get
            {
                return user => new UserStoriesViewModel()
                {
                    Username = user.UserName,
                    AvatarPath = user.UserInfo != null ? user.UserInfo.AvatarPath : "/Images/UsersPictures/default/user.png"
                };
            }
        }

        public IEnumerable<UserFeedStory> Stories { get; set; }
    }
}