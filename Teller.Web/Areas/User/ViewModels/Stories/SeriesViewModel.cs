namespace Teller.Web.Areas.User.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;
    using Teller.Web.Areas.User.ViewModels.Base;
    using Teller.Web.ViewModels.Series;

    public class UserSeriesViewModel : UserViewModel
    {
        public static Expression<Func<AppUser, UserSeriesViewModel>> FromUser
        {
            get
            {
                return user => new UserSeriesViewModel()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    AvatarPath = user.UserInfo != null ? user.UserInfo.AvatarPath : "/Images/UsersPictures/default/user.png"
                };
            }
        }

        public IEnumerable<SeriesViewModel> Series { get; set; }
    }
}