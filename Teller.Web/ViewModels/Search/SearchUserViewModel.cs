namespace Teller.Web.ViewModels.Search
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Teller.Models;

    public class SearchUserViewModel
    {
        public static Expression<Func<AppUser, SearchUserViewModel>> FromUser
        {
            get
            {
                return user => new SearchUserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    AvatarPath = user.UserInfo.AvatarPath
                };
            }
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string AvatarPath { get; set; }
    }
}
