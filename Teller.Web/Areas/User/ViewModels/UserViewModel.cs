namespace Teller.Web.Areas.User.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Teller.Models;

    public class UserViewModel
    {
        public static Expression<Func<AppUser, UserViewModel>> FromUser
        {
            get
            {
                return user => new UserViewModel()
                {
                    Username = user.UserName,
                    RegisteredOn = user.RegisteredOn,
                    Stories = user.Stories,
                    SubscribedTo = user.SubscribedTo,
                    Subscribers = user.Subscribers,
                    Series = user.Series
                };
            }
        }

        public string Username { get; set; }

        public DateTime RegisteredOn { get; set; }

        public UserInfoViewModel UserInfo { get; set; }

        public ICollection<Story> Stories { get; set; }

        public ICollection<Series> Series { get; set; }

        public ICollection<AppUser> Subscribers { get; set; }

        public ICollection<AppUser> SubscribedTo { get; set; }
    }
}