namespace Teller.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Teller.Data;
    using Teller.Web.Areas.User.ViewModels;
    using Teller.Web.Controllers;

    public class StoryController : BaseController
    {
        public StoryController(ITellerData data)
            : base(data)
        {
        }

        public ActionResult Index(string id)
        {
            var user = this.Data.Users.All()
                .Select(UserInfoViewModel.FromUser)
                .SingleOrDefault(u => u.Username == id);

            ViewBag.Username = id;
            ViewBag.AvatarPath = user.AvatarPath;
            return View(user);
        }
    }
}